using API._Repositories;
using API._Services.Interfaces.Manage;
using API.Dtos.Manage.CategoryManagement;
using API.Helpers.Enums;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.Manage
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepositoryAccessor _repositoryAccessor;
        public CategoryService(IRepositoryAccessor repositoryAccessor)
        {
            _repositoryAccessor = repositoryAccessor;
        }

        public async Task<OperationResult> Create(CategoryDetailDto category)
        {
            Category categoryModel = new()
            {
                CateName = $"{category.CateNameVN} - {category.CateNameTW}",
                CateSym = category.CateSym.ToUpper(),
                Visible = category.Visible
            };
            _repositoryAccessor.Category.Add(categoryModel);
            var check = false;
            check = await _repositoryAccessor.SaveChangesAsync();
            //lấy ra cate ID vừa add
            Category cate = await _repositoryAccessor.Category.FirstOrDefaultAsync(x => x.CateName.Trim() == categoryModel.CateName.Trim());

            List<CatLang> catLangs = new()
            {
                new()
                {
                    CateID = cate.CateID,
                    CateName = category.CateNameVN,
                    LanguageID = LangConstants.VN,
                    Position = 0
                },
                new()
                {
                    CateID = cate.CateID,
                    CateName = category.CateNameTW,
                    LanguageID = LangConstants.ZH_TW,
                    Position = 0
                },
                new()
                {
                    CateID = cate.CateID,
                    CateName = category.CateNameEN,
                    LanguageID = LangConstants.EN,
                    Position = 0
                }
            };

            _repositoryAccessor.CatLang.AddMultiple(catLangs);
            check = await _repositoryAccessor.SaveChangesAsync();
            if (check)
            {
                return new OperationResult(true, "Successfully add a category", "Success");
            }
            else
            {
                return new OperationResult(false, "Add a Category Failed", "Failed");
            }
        }

        public async Task<PaginationUtility<CategoryDto>> GetAll(PaginationParam param)
        {
            IQueryable<CategoryDto> data = _repositoryAccessor.Category.FindAll().Select(x => new CategoryDto
            {
                CateID = x.CateID,
                CateName = x.CateName,
                CateSym = x.CateSym,
                Visible = x.Visible
            });
            return await PaginationUtility<CategoryDto>.CreateAsync(data, param.PageNumber, param.PageSize);
        }

        public async Task<CategoryDetailDto> GetEditDetail(int id)
        {
            Category category = await _repositoryAccessor.Category.FindById(id);
            var catLangs = await _repositoryAccessor.CatLang.FindAll(x => x.CateID == id).ToListAsync();
            return new CategoryDetailDto
            {
                CateID = category.CateID,
                CateNameVN = catLangs.FirstOrDefault(x => x.LanguageID == LangConstants.VN)?.CateName,
                CateNameEN = catLangs.FirstOrDefault(x => x.LanguageID == LangConstants.EN)?.CateName,
                CateNameTW = catLangs.FirstOrDefault(x => x.LanguageID == LangConstants.ZH_TW)?.CateName,
                CateSym = category.CateSym,
                Visible = category.Visible
            };

        }

        public async Task<OperationResult> Update(CategoryDetailDto category)
        {
            List<CatLang> catLangs = await _repositoryAccessor.CatLang.FindAll(x => x.CateID == category.CateID).ToListAsync();
            catLangs.FirstOrDefault(x => x.LanguageID == LangConstants.VN).CateName = category.CateNameVN;
            catLangs.FirstOrDefault(x => x.LanguageID == LangConstants.EN).CateName = category.CateNameEN;
            catLangs.FirstOrDefault(x => x.LanguageID == LangConstants.ZH_TW).CateName = category.CateNameTW;
            var categoryModel = await _repositoryAccessor.Category.FindById(category.CateID);
            categoryModel.CateName = $"{category.CateNameVN} - {category.CateNameTW}";
            categoryModel.CateSym = category.CateSym;
            categoryModel.Visible = category.Visible;
            _repositoryAccessor.Category.Update(categoryModel);
            _repositoryAccessor.CatLang.UpdateMultiple(catLangs);
            if (await _repositoryAccessor.SaveChangesAsync())
            {
                return new OperationResult(true, "Successfully update a category", "Success");
            }
            else
            {
                return new OperationResult(false, "Update a Category Failed", "Failed");
            }
        }
    }
}