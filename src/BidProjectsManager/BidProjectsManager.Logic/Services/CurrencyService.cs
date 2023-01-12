using AutoMapper;
using AutoMapper.QueryableExtensions;
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
using System.Runtime.CompilerServices;

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
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IValidator<CreateCurrencyCommand> _createCurrencyValidator;
        private readonly IValidator<UpdateCurrencyCommand> _updateCurrencyValidator;
        private readonly IMapper _mapper;

        public CurrencyService(ICurrencyRepository currencyRepository, IValidator<CreateCurrencyCommand> createCurrencyValidator, IValidator<UpdateCurrencyCommand> updateCurrencyValidator, IMapper mapper)
        {
            _currencyRepository = currencyRepository;
            _createCurrencyValidator = createCurrencyValidator;
            _updateCurrencyValidator = updateCurrencyValidator;
            _mapper = mapper;
        }

        public async Task<PaginatedList<CurrencyListItemDto>> GetCurrencies(CurrencyQuery query)
        {
            var currencies = _currencyRepository.GetAll().Include(x => x.Countries).AsNoTracking();

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
            => await _currencyRepository.GetAll().OrderBy(x => x.Code).ProjectTo<CurrencyDto>(_mapper.ConfigurationProvider).ToListAsync();

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
                    _currencyRepository.Add(currency);
                    await _currencyRepository.SaveChangesAsync();
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
                    var currency = await _currencyRepository.GetById(command.Id).FirstOrDefaultAsync();
                    currency.Name = command.Name;
                    currency.Code = command.Code;
                    _currencyRepository.Update(currency);
                    await _currencyRepository.SaveChangesAsync();
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
                var existProjectsWithThisCurrency = await _currencyRepository.GetById(id).Include(x => x.Projects).Select(x => x.Projects.Any()).FirstOrDefaultAsync();
                if(existProjectsWithThisCurrency)
                {
                    return false;
                }
                var existCountriesWithThisCurrency = await _currencyRepository.GetById(id).Include(x => x.Countries).Select(x => x.Countries.Any()).FirstOrDefaultAsync();
                if (existCountriesWithThisCurrency)
                {
                    return false;
                }
                _currencyRepository.Delete(id);
                await _currencyRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
