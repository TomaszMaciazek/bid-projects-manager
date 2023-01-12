using AutoMapper;
using AutoMapper.QueryableExtensions;
using BidProjectsManager.DataLayer.Common;
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
    public interface ICurrencyService
    {
        Task<bool> CreateCurrencyAsync(CreateCurrencyCommand command);
        Task<bool> DeleteCurrencyAsync(int id);
        Task<IList<CurrencyDto>> GetAllCurrenciesAsync();
        Task<PaginatedList<CurrencyListItemDto>> GetCurrencies(CurrencyQuery query);
        Task<bool> UpdateCurrencyAsync(UpdateCurrencyCommand command);
    }

    public class CurrencyService : ICurrencyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateCurrencyCommand> _createCurrencyValidator;
        private readonly IValidator<UpdateCurrencyCommand> _updateCurrencyValidator;
        private readonly IMapper _mapper;

        public CurrencyService(IUnitOfWork unitOfWork, IValidator<CreateCurrencyCommand> createCurrencyValidator, IValidator<UpdateCurrencyCommand> updateCurrencyValidator, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _createCurrencyValidator = createCurrencyValidator;
            _updateCurrencyValidator = updateCurrencyValidator;
            _mapper = mapper;
        }

        public async Task<PaginatedList<CurrencyListItemDto>> GetCurrencies(CurrencyQuery query)
        {
            var currencies = _unitOfWork.CurrencyRepository.GetAll().Include(x => x.Countries).AsNoTracking();

            currencies = !string.IsNullOrEmpty(query.Name)
                ? currencies.Where(x => x.Name.ToLower().Contains(query.Name.ToLower()))
                : currencies;

            if (query.SortOption.HasValue)
            {
                if (query.SortOption.Value == CurrencySortOption.NameAscending)
                    currencies = currencies.OrderBy(x => x.Name);
                else if (query.SortOption.Value == CurrencySortOption.NameDescending)
                    currencies = currencies.OrderByDescending(x => x.Name);
                else if (query.SortOption.Value == CurrencySortOption.CodeDescending)
                    currencies = currencies.OrderByDescending(x => x.Code);
                else
                    currencies = currencies.OrderBy(x => x.Code);

            }

            return await currencies.ProjectTo<CurrencyListItemDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(query.PageNumber, query.PageSize);
        }

        public async Task<IList<CurrencyDto>> GetAllCurrenciesAsync()
            => await _unitOfWork.CurrencyRepository.GetAll().OrderBy(x => x.Code).ProjectTo<CurrencyDto>(_mapper.ConfigurationProvider).ToListAsync();

        public async Task<bool> CreateCurrencyAsync(CreateCurrencyCommand command)
        {
            try
            {
                var validationResult = await _createCurrencyValidator.ValidateAsync(command);
                if (validationResult.IsValid)
                {
                    var currency = new Currency
                    {
                        Code = command.Code.ToUpper(),
                        Name = command.Name
                    };
                    _unitOfWork.CurrencyRepository.Add(currency);
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

        public async Task<bool> UpdateCurrencyAsync(UpdateCurrencyCommand command)
        {
            try
            {
                var validationResult = await _updateCurrencyValidator.ValidateAsync(command);
                if (validationResult.IsValid)
                {
                    var currency = await _unitOfWork.CurrencyRepository.GetById(command.Id).FirstOrDefaultAsync();
                    currency.Name = command.Name;
                    currency.Code = command.Code;
                    _unitOfWork.CurrencyRepository.Update(currency);
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

        public async Task<bool> DeleteCurrencyAsync(int id)
        {
            try
            {
                var existProjectsWithThisCurrency = await _unitOfWork.CurrencyRepository.GetById(id).Include(x => x.Projects).Select(x => x.Projects.Any()).FirstOrDefaultAsync();
                if(existProjectsWithThisCurrency)
                {
                    return false;
                }
                var existCountriesWithThisCurrency = await _unitOfWork.CurrencyRepository.GetById(id).Include(x => x.Countries).Select(x => x.Countries.Any()).FirstOrDefaultAsync();
                if (existCountriesWithThisCurrency)
                {
                    return false;
                }
                _unitOfWork.CurrencyRepository.Delete(id);
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
