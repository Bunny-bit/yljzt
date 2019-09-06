using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using QC.MF.Authorization;
using QC.MF.Authorization.Users;
using DataExporting;
using DataExporting.Dto;
using Abp.Application.Services;
using Abp.Domain.Entities;
using QC.MF.CommonDto;
using System;
using System.Linq.Expressions;
using QC.MF.Commons;

namespace QC.MF
{
    public class AsyncMFCrudAppService<TEntity, TEntityDto, TGetAllInput, TCreateInput, TUpdateInput> :
        AsyncMFCrudAppService<
            TEntity,
            TEntityDto,
            int,
            TGetAllInput,
            TCreateInput,
            TUpdateInput>
        where TEntity : class, IEntity<int>
        where TEntityDto : IEntityDto<int>
        where TUpdateInput : IEntityDto<int>
    {

        public AsyncMFCrudAppService(IRepository<TEntity, int> repository) : base(repository)
        {
        }
    }
    public class AsyncMFCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput> :
        AsyncCrudAppService<
            TEntity,
            TEntityDto,
            TPrimaryKey,
            TGetAllInput,
            TCreateInput,
            TUpdateInput>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {
        
        public IAppFolders AppFolders { get; set; }


        private string _managePermissionName;
        public string ManagePermissionName
        {
            get { return _managePermissionName; }
            set
            {
                _managePermissionName = value;
                CreatePermissionName = _managePermissionName;
                UpdatePermissionName = _managePermissionName;
                DeletePermissionName = _managePermissionName;
                GetAllPermissionName = _managePermissionName;
            }
        }

        public AsyncMFCrudAppService(
            IRepository<TEntity, TPrimaryKey> repository
        ) : base(repository)
        {
        }

        protected IQueryable<TEntity> ApplyFilter(IQueryable<TEntity> q, object input)
        {
            if (input is IFilteredResultRequest _input)
            {
                if (!string.IsNullOrWhiteSpace(_input.Filter))
                {
                    q = q.Filter(_input.Filter);
                }
            }
            return q;
        }

        protected override IQueryable<TEntity> CreateFilteredQuery(TGetAllInput input)
        {
            var q = base.CreateFilteredQuery(input);

            q = ApplyFilter(q, input);
            return q;
        }

        public override async Task<TEntityDto> Create(TCreateInput input)
        {
            CheckCreatePermission();
            var _data = input.MapTo<TEntity>();
            await Repository.InsertAsync(_data);
            await UnitOfWorkManager.Current.SaveChangesAsync();

            //(object)x.Id == (object)_data.Id 如果用于内存查询就会出错，当前是用于数据库查询
            var dataR = await Repository.GetAll().AsNoTracking().FirstAsync(x => (object)x.Id == (object)_data.Id);
            return dataR.MapTo<TEntityDto>();
        }

        public override async Task<TEntityDto> Update(TUpdateInput input)
        {
            CheckUpdatePermission();

            var entity = await GetEntityByIdAsync(input.Id);

            MapToEntity(input, entity);
            await CurrentUnitOfWork.SaveChangesAsync();

            var dataR = await Repository.GetAll().AsNoTracking().FirstAsync(x => (object)x.Id == (object)input.Id);
            return MapToEntityDto(dataR);
        }
        private void CheckDelete(TEntity entity)
        {
            if (entity is ICanDelete canDeleteObj)
            {
                if (canDeleteObj is ICheckDelete checkDeleteObj)
                {
                    checkDeleteObj.CheckDelete();
                }
                else if (!canDeleteObj.CanDelete())
                {
                    throw new Abp.UI.UserFriendlyException("存在引用关系，不能删除。");
                }
            }
        }
        public override async Task Delete(EntityDto<TPrimaryKey> input)
        {
            CheckDeletePermission();
            var obj = await Repository.FirstOrDefaultAsync(input.Id);
            if (obj == null) { return; }
            CheckDelete(obj);
            await Repository.DeleteAsync(obj);
        }
        public async Task DeleteBatch(ArrayDto<TPrimaryKey> input)
        {
            CheckDeletePermission();
            var objs = await Repository.GetAllListAsync(x => input.Value.Contains(x.Id));
            foreach (var item in objs)
            {
                CheckDelete(item);
                await Repository.DeleteAsync(item);
            }
        }

        /// <summary>
        /// 获取下拉列表
        /// </summary>
        protected async Task<List<NameValueDto<T>>> GetDropDownList<T>(Expression<Func<TEntity, NameValueDto<T>>> selector, Expression<Func<TEntity, bool>> predicate = null, string sort = null)
        {
            return await GetDropDownList(selector, x => x.WhereIf(predicate != null, predicate));
        }
        /// <summary>
        /// 获取下拉列表
        /// </summary>
        protected async Task<List<NameValueDto<T>>> GetDropDownList<T>(Expression<Func<TEntity, NameValueDto<T>>> selector, Func<IQueryable<TEntity>, IQueryable<TEntity>> filter, string sort = null)
        {
            var q = Repository.GetAll();
            if (!string.IsNullOrEmpty(sort))
            {
                q = q.OrderBy(sort);
            }
            if (filter != null)
            {
                q = filter(q);
            }
            return await q
                .Select(selector)
                .ToListAsync();
        }

        public virtual async Task<FileDto> ExportToExcel(FilteredInputDto input, Func<IQueryable<TEntity>, IQueryable<TEntity>> where)
        {
            var q = Repository.GetAll();
            if (where != null)
            {
                q = where(q);
            }
            q = ApplyFilter(q, input);
            var data = (await q.ToListAsync()).MapTo<List<TEntityDto>>();
            FileDto fileinfo = new ExcelExporter().ExportToFile(data, AppFolders.TempFileDownloadFolder);
            return fileinfo;
        }
        public virtual async Task<FileDto> ExportToExcel(FilteredInputDto input)
        {
            return await ExportToExcel(input, null);
        }
    }
}
