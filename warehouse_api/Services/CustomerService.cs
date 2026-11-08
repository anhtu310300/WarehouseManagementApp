using AutoMapper;
using warehouse_api.Domain;
using warehouse_api.DTOs.Customers;
using warehouse_api.Repositories.Interfaces;
using warehouse_api.Services.Interfaces;

namespace warehouse_api.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository
            _customerRepository;

        private readonly IMapper _mapper;

        public CustomerService(
            ICustomerRepository customerRepository,
            IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<List<CustomerResponseDto>>
            GetAllAsync()
        {
            var customers =
                await _customerRepository.GetAllAsync();

            return _mapper.Map<List<CustomerResponseDto>>(
                customers);
        }

        public async Task<CustomerResponseDto?> GetByIdAsync(
            int customerId)
        {
            var customer =
                await _customerRepository.GetByIdAsync(
                    customerId);

            if (customer is null)
            {
                return null;
            }

            return _mapper.Map<CustomerResponseDto>(customer);
        }

        public async Task<CustomerResponseDto> CreateAsync(
            CreateCustomerRequestDto request)
        {
            var customer =
                _mapper.Map<Customer>(request);

            var createdCustomer =
                await _customerRepository.CreateAsync(customer);

            return _mapper.Map<CustomerResponseDto>(
                createdCustomer);
        }

        public async Task<CustomerResponseDto?> UpdateAsync(
            int customerId,
            UpdateCustomerRequestDto request)
        {
            var customer =
                await _customerRepository.GetByIdAsync(
                    customerId);

            if (customer is null)
            {
                return null;
            }

            _mapper.Map(request, customer);

            var updatedCustomer =
                await _customerRepository.UpdateAsync(customer);

            return _mapper.Map<CustomerResponseDto>(
                updatedCustomer);
        }

        public async Task<bool> DeleteAsync(int customerId)
        {
            return await _customerRepository.DeleteAsync(
                customerId);
        }
    }
}
