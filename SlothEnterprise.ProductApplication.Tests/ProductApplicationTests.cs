using FluentAssertions;
using Moq;
using SlothEnterprise.External;
using SlothEnterprise.External.V1;
using SlothEnterprise.ProductApplication.Applications;
using SlothEnterprise.ProductApplication.Products;
using Xunit;

namespace SlothEnterprise.ProductApplication.Tests
{
    public class ProductApplicationTests
    {
        private readonly IProductApplicationService _sut;
        private readonly Mock<ISelectInvoiceService> _selectiveInvoiceServiceMock = new Mock<ISelectInvoiceService>();
        private readonly Mock<IConfidentialInvoiceService> _confidentialInvoiceServiceMock = new Mock<IConfidentialInvoiceService>();
        private readonly Mock<IBusinessLoansService> _businessLoansServiceMock = new Mock<IBusinessLoansService>();
        private readonly ISellerApplication _confidentialInvoiceDiscount;
        private readonly ISellerApplication _selectiveInvoice;
        private readonly ISellerApplication _businessLoans;

        private readonly Mock<IApplicationResult> _result = new Mock<IApplicationResult>();
        private int _resultSelectInvoice = 0;

        public ProductApplicationTests()
        {
            _result.SetupProperty(p => p.ApplicationId, 1);
            _result.SetupProperty(p => p.Success, true);
            var productApplicationService = new Mock<IProductApplicationService>();
            _sut = productApplicationService.Object;
            productApplicationService.Setup(m => m.SubmitApplicationFor(It.IsAny<ISellerApplication>())).Returns(1);

            ///////////////////////////////////////////////////////////
            // Confidential Invoice Discount application Setup
            var confidentialInvoiceDiscountApplication = new Mock<ISellerApplication>();
            confidentialInvoiceDiscountApplication.SetupProperty(p => p.Product, new ConfidentialInvoiceDiscount());
            confidentialInvoiceDiscountApplication.SetupProperty(p => p.CompanyData, new SellerCompanyData());
            
            _confidentialInvoiceDiscount = confidentialInvoiceDiscountApplication.Object;
            ///////////////////////////////////////////////////////
            
            ///////////////////////////////////////////////////////
            // Selective Invoice Application
            var selectiveInvoiceApplication = new Mock<ISellerApplication>();
            selectiveInvoiceApplication.SetupProperty(p => p.Product, new SelectiveInvoiceDiscount());
            selectiveInvoiceApplication.SetupProperty(p => p.CompanyData, new SellerCompanyData());

            _selectiveInvoice = selectiveInvoiceApplication.Object;
            ///////////////////////////////////////////////////////

            //////////////////////////////////////////////////////
            /// 
            var businessLoansApplication = new Mock<ISellerApplication>();
            businessLoansApplication.SetupProperty(p => p.Product, new BusinessLoans());
            businessLoansApplication.SetupProperty(p => p.CompanyData, new SellerCompanyData());

            _businessLoans = businessLoansApplication.Object;
            ////////////////////////////////////////////////////////
        }

        [Fact]
        public void ProductApplicationService_SubmitApplicationFor_WhenCalledWithSelectiveInvoiceDiscount_ShouldReturnOne()
        {
            _selectiveInvoiceServiceMock.Setup(m => m.SubmitApplicationFor("128974", It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(_resultSelectInvoice);
            var result = _sut.SubmitApplicationFor(_selectiveInvoice);
            result.Should().Be(1);
        }

        [Fact]
        public void ProductApplicationService_SubmitApplicationFor_WhenCalledWithConfidentialInvoiceDiscountDiscount_ShouldReturnOne()
        {
            _confidentialInvoiceServiceMock.Setup(m => m.SubmitApplicationFor(It.IsAny<CompanyDataRequest>(), It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(_result.Object);
            var result = _sut.SubmitApplicationFor(_confidentialInvoiceDiscount);
            result.Should().Be(1);
        }

        [Fact]
        public void ProductApplicationService_SubmitApplicationFor_WhenCalledWithBusinessLoans_ShouldReturnOne()
        {
            _businessLoansServiceMock.Setup(m => m.SubmitApplicationFor(It.IsAny<CompanyDataRequest>(), new LoansRequest() {InterestRatePerAnnum= It.IsAny<decimal>(), LoanAmount= It.IsAny<decimal>() })).Returns(_result.Object);
            var result = _sut.SubmitApplicationFor(_businessLoans);
            result.Should().Be(1);
        }


    }
}