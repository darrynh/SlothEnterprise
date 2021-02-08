using System;
using SlothEnterprise.External;
using SlothEnterprise.External.V1;
using SlothEnterprise.ProductApplication.Applications;
using SlothEnterprise.ProductApplication.Products;

namespace SlothEnterprise.ProductApplication
{
    public class ProductApplicationService : IProductApplicationService
    {
        private readonly ISelectInvoiceService _selectInvoiceService;
        private readonly IConfidentialInvoiceService _confidentialInvoiceWebService;
        private readonly IBusinessLoansService _businessLoansService;

        public ProductApplicationService(ISelectInvoiceService selectInvoiceService, IConfidentialInvoiceService confidentialInvoiceWebService, IBusinessLoansService businessLoansService)
        {
            _selectInvoiceService = selectInvoiceService;
            _confidentialInvoiceWebService = confidentialInvoiceWebService;
            _businessLoansService = businessLoansService;
        }

        public int SubmitApplicationFor(ISellerApplication application)
        {

            if(application.SelectiveInvoiceDiscount != null)
                return _selectInvoiceService.SubmitApplicationFor(application.CompanyDataRequest.CompanyNumber.ToString(), application.SelectiveInvoiceDiscount.InvoiceAmount, application.SelectiveInvoiceDiscount.AdvancePercentage);

            if (application.ConfidentialInvoiceDiscount != null)
            {
                var result = _confidentialInvoiceWebService.SubmitApplicationFor(application.CompanyDataRequest, application.ConfidentialInvoiceDiscount.TotalLedgerNetworth, application.ConfidentialInvoiceDiscount.AdvancePercentage, application.ConfidentialInvoiceDiscount.VatRate);
                return (result.Success) ? result.ApplicationId ?? -1 : -1;
            }

            if(application.BusinessLoans != null)
            {
                var result = _businessLoansService.SubmitApplicationFor(application.CompanyDataRequest, application.LoansRequest);
                return (result.Success) ? result.ApplicationId ?? -1 : -1;
            }

            throw new InvalidOperationException();
        }
    }
}
