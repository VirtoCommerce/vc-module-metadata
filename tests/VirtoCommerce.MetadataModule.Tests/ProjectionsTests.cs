using System;
using System.Collections.Generic;
using System.Text;
using VirtoCommerce.MetadataModule.Core.Model;
using VirtoCommerce.MetadataModule.Data.Services;
using VirtoCommerce.OrdersModule.Core.Model;
using Xunit;

namespace VirtoCommerce.MetadataModule.Tests
{
    public class ProjectionsTests
    {
        private readonly MetadataService metadataService = new MetadataService();

        [Fact]
        public void TestAddEnlist()
        {
            // All order properties projection (full, no details)
            var projectionDefault = new Projection(typeof(CustomerOrder), ReadType.OnlyThat)
            {
                Name = "Default"
            };
            metadataService.AddProjection(projectionDefault);

            // All order properties projection (full + 1st level details)
            var projectionDefaultDetailsRelated = new Projection(typeof(CustomerOrder), ReadType.RelatedDetails)
            {
                Name = "DefaultDetailsRelated1stLevel"
            };
            metadataService.AddProjection(projectionDefaultDetailsRelated);

            // An example with main order info (as example -- for orders list)
            var projectionShortInfoList = new Projection
            {
                DefineClassType = typeof(CustomerOrder),
                Name = "ShortInfoList"
            };
            projectionShortInfoList.AddProperties(new string[] {
                nameof(CustomerOrder.StoreId),
                nameof(CustomerOrder.Id),
                nameof(CustomerOrder.CustomerName),
                nameof(CustomerOrder.OrganizationName),
                nameof(CustomerOrder.CreatedDate),
                nameof(CustomerOrder.CreatedBy),
                nameof(CustomerOrder.Sum),
                nameof(CustomerOrder.Status)                
            });            
            metadataService.AddProjection(projectionShortInfoList);

            // An example with main order info (as example -- payments edit)
            var projectionWithLineItems = new Projection
            {
                DefineClassType = typeof(CustomerOrder),
                Name = "WithLineItems"
            };
            projectionWithLineItems.AddProperties(new string[] {
                nameof(CustomerOrder.StoreId),
                nameof(CustomerOrder.Id),
                nameof(CustomerOrder.CustomerName),
                nameof(CustomerOrder.OrganizationName),
                nameof(CustomerOrder.CreatedDate),
                nameof(CustomerOrder.CreatedBy),
                nameof(CustomerOrder.Sum),
                nameof(CustomerOrder.Status),
                nameof(CustomerOrder.Currency),
                nameof(CustomerOrder.Total),
            });
            projectionWithLineItems.AddDetailInProjection(nameof(CustomerOrder.Items), new Projection(typeof(LineItem), ReadType.OnlyThat), true);
            projectionWithLineItems.AddDetailInProjection(nameof(CustomerOrder.InPayments), new Projection(typeof(PaymentIn), ReadType.OnlyThat), true);
            metadataService.AddProjection(projectionWithLineItems);



            var aa = metadataService.GetModels();
            var bb = metadataService.GetProjections(typeof(CustomerOrder));
        }
    }
}
