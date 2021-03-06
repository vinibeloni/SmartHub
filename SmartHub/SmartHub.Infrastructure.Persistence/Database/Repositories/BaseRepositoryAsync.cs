﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartHub.Application.Common.Interfaces.Database;
using SmartHub.Domain.Entities;
using SmartHub.Infrastructure.Database;

namespace SmartHub.Infrastructure.Persistence.Database.Repositories
{
	public class BaseRepositoryAsync<T> : IBaseRepository<T> where T : BaseEntity
	{
		private readonly DbSet<T> _entities;
		private readonly AppDbContext _appDbContext;

		public BaseRepositoryAsync(AppDbContext appDbContext)
		{
			_entities = appDbContext.Set<T>();
			_appDbContext = appDbContext;
		}

		#region Getter

		public async Task<T> GetByIdAsync(string id)
		{
			return await _entities.FindAsync(id);
		}

		public async Task<List<T>> GetAllAsync()
		{
			return await _entities.ToListAsync().ConfigureAwait(false);
		}

		public async Task<List<T>> FindAllAsync(Expression<Func<T, bool>> expression)
		{
			return await _entities.Where(expression).ToListAsync();
		}

		public async Task<T> FindbyAsync(Expression<Func<T, bool>> expression)
		{
			return await _entities.Where(expression).FirstOrDefaultAsync();
		}

		#endregion Getter

		#region Add

		public async Task<bool> AddAsync(T entity)
		{
			try
			{
				await _entities.AddAsync(entity);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public async Task<bool> AddRangeAsync(IEnumerable<T> entities)
		{
			try
			{
				await _entities.AddRangeAsync(entities);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		#endregion Add

		#region Update

		public async Task<bool> UpdateAsync(T entity)
		{
			try
			{
				_entities.Attach(entity);
				_appDbContext.Entry(entity).State = EntityState.Modified;
				return true;
			}
			catch (Exception)
			{
				return await Task.FromResult(false).ConfigureAwait(false);
			}
		}

		public async Task<bool> UpdateRangeAsync(IEnumerable<T> entities)
		{
			try
			{
				_entities.AttachRange(entities);
				_appDbContext.Entry(entities).State = EntityState.Modified;
				return await Task.FromResult(true).ConfigureAwait(false);
			}
			catch (Exception)
			{
				return await Task.FromResult(false).ConfigureAwait(false);
			}
		}

		#endregion Update

		#region Remove
		public async Task<bool> RemoveAsync(T entity)
		{
			try
			{
				_entities.Remove(entity);
				return await Task.FromResult(true).ConfigureAwait(false);
			}
			catch (Exception)
			{
				return await Task.FromResult(false).ConfigureAwait(false);
			}
		}

		public async Task<bool> RemoveRangeAsync(IEnumerable<T> entities)
		{
			try
			{
				_entities.RemoveRange(entities);
				return await Task.FromResult(true).ConfigureAwait(false);
			}
			catch (Exception)
			{
				return await Task.FromResult(false).ConfigureAwait(false);
			}
		}

		#endregion Remove
	}
}
