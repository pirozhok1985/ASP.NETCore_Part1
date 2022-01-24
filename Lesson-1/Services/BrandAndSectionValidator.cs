using System.ComponentModel.DataAnnotations;
using WebStore.Services.Database;
using WebStore.Services.Interfaces;

namespace WebStore.Services;

public class BrandAndSectionValidator
{
    public static ValidationResult ValidateSection(string sectionName, ValidationContext validationContext)
    {
        var productData = (ProductDataDB)validationContext.GetRequiredService<IProductData>();
        return productData.GetSections().Any(s=> s.Name.Equals(sectionName)) ? ValidationResult.Success : new ValidationResult("Секция с таким наименованием отсутствует.");
    }
    public static ValidationResult ValidateBrand(string brandName, ValidationContext validationContext)
    {
        var productData = (ProductDataDB)validationContext.GetRequiredService<IProductData>();
        return productData.GetBrands().Any(b=> b.Name.Equals(brandName)) ? ValidationResult.Success : new ValidationResult("Бренд с таким наименованием отсутствует.");
    }
}