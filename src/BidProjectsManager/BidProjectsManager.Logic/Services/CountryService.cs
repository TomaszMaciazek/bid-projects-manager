using AutoMapper;
using AutoMapper.QueryableExtensions;
using BidProjectsManager.DataLayer.Common;
using BidProjectsManager.DataLayer.Repositories;
using BidProjectsManager.Logic.Extensions;
using BidProjectsManager.Logic.Result;
using BidProjectsManager.Model.Commands;
using BidProjectsManager.Model.Dto;
using BidProjectsManager.Model.Entities;
using BidProjectsManager.Model.Enums;
using BidProjectsManager.Model.Queries;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BidProjectsManager.Logic.Services
{
    public interface ICountryService
    {
        Task<bool> CreateCountryAsync(CreateCountryCommand command);
        Task<bool> DeleteCountryAsync(int id);
        Task<IList<CountryDto>> GetAllCountriesAsync();
        Task<PaginatedList<CountryListItemDto>> GetCountriesAsync(CountryQuery query);
        Task<bool> UpdateCountryAsync(UpdateCountryCommand command);
    }

    public class CountryService : ICountryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateCountryCommand> _createCountryValidator;
        private readonly IValidator<UpdateCountryCommand> _updateCountryValidator;
        private readonly IMapper _mapper;

        public CountryService(IUnitOfWork unitOfWork, IValidator<CreateCountryCommand> createCountryValidator, IValidator<UpdateCountryCommand> updateCountryValidator, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _createCountryValidator = createCountryValidator;
            _updateCountryValidator = updateCountryValidator;
            _mapper = mapper;
        }

        public async Task<PaginatedList<CountryListItemDto>> GetCountriesAsync(CountryQuery query)
        {
            var countries = _unitOfWork.CountryRepository.GetAll().Include(x => x.Projects)
                .AsNoTracking();

            countries = !string.IsNullOrEmpty(query.Name) 
                ? countries.Where(x => x.Name.ToLower().Contains(query.Name.ToLower()))
                : countries;

            if (query.SortOption.HasValue)
            {
                if(query.SortOption.Value == CountrySortOption.NameAscending)
                    countries = countries.OrderBy(x => x.Name);
                else if (query.SortOption.Value == CountrySortOption.NameDescending)
                    countries = countries.OrderByDescending(x => x.Name);
                else if (query.SortOption.Value == CountrySortOption.CodeDescending)
                    countries = countries.OrderByDescending(x => x.Code);
                else
                    countries = countries.OrderBy(x => x.Code);

            }
            else
            {
                countries = countries.OrderBy(x => x.Name);
            }

            return await countries.ProjectTo<CountryListItemDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(query.PageNumber, query.PageSize);
        }

        public async Task<IList<CountryDto>> GetAllCountriesAsync()
            => await _unitOfWork.CountryRepository.GetAll().ProjectTo<CountryDto>(_mapper.ConfigurationProvider).ToListAsync();

        public async Task<bool> CreateCountryAsync(CreateCountryCommand command)
        {
            try
            {
                var validationResult = await _createCountryValidator.ValidateAsync(command);
                if (validationResult.IsValid)
                {
                    var country = new Country
                    {
                        Code = command.Code,
                        CurrencyId = command.CurrencyId,
                        Name = command.Name
                    };
                    _unitOfWork.CountryRepository.Add(country);
                    await _unitOfWork.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateCountryAsync(UpdateCountryCommand command)
        {
            try
            {
                var validationResult = await _updateCountryValidator.ValidateAsync(command);
                if (validationResult.IsValid)
                {
                    var country = await _unitOfWork.CountryRepository.GetById(command.Id).FirstOrDefaultAsync();
                    country.Name = command.Name;
                    country.Code = command.Code.ToUpper();
                    country.CurrencyId = command.CurrencyId;
                    _unitOfWork.CountryRepository.Update(country);
                    await _unitOfWork.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteCountryAsync(int id)
        {
            try
            {
                var existProjectsWithThisCountry = await _unitOfWork.CountryRepository.GetById(id).Include(x => x.Projects).Select(x => x.Projects.Any()).FirstOrDefaultAsync();
                if (existProjectsWithThisCountry)
                {
                    return false;
                }
                _unitOfWork.CountryRepository.Delete(id);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
