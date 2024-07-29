using AutoMapper;
using CRM.Domain.Commands.Payment;
using CRM.Domain.Entities;
using CRM.Domain.Responses.Payment;

namespace CRM.WebApi.Mapping;

public class PaymentProfile : Profile
{
    public PaymentProfile()
    {
        CreateMap<CreatePaymentCommand, Payment>();
        CreateMap<UpdatePaymentCommand, Payment>();
        CreateMap<Payment, PaymentResponse>();
    }
}