using SlothEnterprise.External;
using SlothEnterprise.ProductApplication.Products;
using System.Collections.Generic;

namespace SlothEnterprise.ProductApplication.Applications
{

    public interface ISellerApplication
    {
        IProduct Product { get; set; }
        ISellerCompanyData CompanyData { get; set; }
        CompanyDataRequest CompanyDataRequest { get; }
        SelectiveInvoiceDiscount SelectiveInvoiceDiscount { get; }
        ConfidentialInvoiceDiscount ConfidentialInvoiceDiscount { get; }
        BusinessLoans BusinessLoans { get; }

        LoansRequest LoansRequest { get; }
    }

    public class SellerApplication : ISellerApplication
    {
        public IProduct Product { get; set; }
        public ISellerCompanyData CompanyData { get; set; }

        public SelectiveInvoiceDiscount SelectiveInvoiceDiscount
        {
            get
            {
                return Product as SelectiveInvoiceDiscount; // using as will return null if it cant cast it.
            }
            
        }

        public LoansRequest LoansRequest 
        {
            get
            {
                if (BusinessLoans != null)
                    return new LoansRequest() { InterestRatePerAnnum = BusinessLoans.InterestRatePerAnnum, LoanAmount = BusinessLoans.LoanAmount };
               
                return null;
            } 
        
        }

        public ConfidentialInvoiceDiscount ConfidentialInvoiceDiscount
        {
            get
            {
                return Product as ConfidentialInvoiceDiscount;
            }
        }

        public BusinessLoans BusinessLoans
        {
            get
            {
                return Product as BusinessLoans;
            }
        }

        public CompanyDataRequest CompanyDataRequest
        {
            get
            {
                if(CompanyData != null) 
                    return new CompanyDataRequest()
                { 
                    CompanyFounded = CompanyData.Founded,
                    CompanyNumber = CompanyData.Number,
                    CompanyName = CompanyData.Name,
                    DirectorName = CompanyData.DirectorName
                };

                return new CompanyDataRequest();
            }
        }
    }
}