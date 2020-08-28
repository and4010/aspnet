using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CHPOUTSRCMES.TASK.Models.Repository.Interface;
using Dapper;


namespace CHPOUTSRCMES.TASK.Models.Repository.MsSql
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        public string IdField { set; get; }

        #region Context Property

        private string _tableName;

        private IDbConnection dbConnection;

        #endregion

        
        protected IDbConnection Connection
        { 
            set{ dbConnection = value; } 
            get{ return dbConnection; } 
        }

        protected string tableName 
        { 
            set{ _tableName = value; } 
            get{ return _tableName; } 
        }

        #region Constructor
        public GenericRepository()
        {
            IdField = "Id";
        }

        public GenericRepository(IDbConnection conn, string tableName)
        {
            IdField = "Id";
            Connection = conn;
            this.tableName = tableName;
        }

        #endregion

        #region Generic Repository


        private IEnumerable<PropertyInfo> GetProperties()
        {
            return typeof(T).GetProperties();
        }

        public async Task<IEnumerable<T>> GetAllAsync(IDbTransaction transaction = null)
        {
            return await Connection.QueryAsync<T>(GenerateSelectQuery(), transaction: transaction);
        }


        public async Task DeleteRowAsync(long id, IDbTransaction transaction = null)
        {
                await Connection.ExecuteAsync($"DELETE FROM {tableName} WHERE {IdField}=@Id", new { Id = id }, transaction: transaction);
        }

        public async Task<T> GetAsync(long id, IDbTransaction transaction = null)
        {
            var result = await Connection.QuerySingleOrDefaultAsync<T>($"{GenerateSelectQuery()} WHERE {IdField}=@Id", new { Id = id }, transaction: transaction);
            if (result == null)
                throw new KeyNotFoundException($"{tableName} with id [{id}] could not be found.");

            return result;
        }

        public async Task<int> SaveRangeAsync(IEnumerable<T> list, IDbTransaction transaction = null)
        {
            var query = GenerateInsertQuery();
            return await Connection.ExecuteAsync(query, list, transaction: transaction);
        }

        public async Task<long?> InsertAsync(T entity, IDbTransaction transaction = null)
        {
            var insertQuery = GenerateInsertQuery();
            long? id = null;
            id = await Connection.QuerySingleAsync<long>(insertQuery, entity, transaction: transaction);

            return id;
        }

        public async Task UpdateAsync(T entity, IDbTransaction transaction = null)
        {
            await Connection.ExecuteAsync(GenerateUpdateQuery(), entity, transaction: transaction);
        }

        public async Task<int> CountAsync(IDbTransaction transaction = null)
        {
            return await Connection.QuerySingleAsync<int>($"SELECT COUNT(*) FROM {tableName}", transaction: transaction);
        }

        public async Task TruncateAsync(IDbTransaction transaction = null)
        {
            await Connection.ExecuteAsync($"TRUNCATE FROM [{tableName}]", transaction: transaction);
        }

        protected string GenerateSelectQuery()
        {
            var selectQuery = new StringBuilder("SELECT ");


            var properties = GenerateListOfProperties(GetProperties());
            properties.ForEach(prop =>
            {
                selectQuery.Append($"{prop},");
            });

            selectQuery
                .Remove(selectQuery.Length - 1, 1)
                .Append($" FROM {tableName}");

            return selectQuery.ToString();
        }

        private string GenerateInsertQuery()
        {
            var insertQuery = new StringBuilder($"INSERT INTO {tableName} ");
            
            insertQuery.Append("(");

            var properties = GenerateListOfProperties(GetProperties());
            properties.ForEach(prop =>
            {
                if (!prop.Equals(IdField))
                { 
                    insertQuery.Append(string.Format("{0},", prop)); 
                }
            });

            insertQuery
                .Remove(insertQuery.Length - 1, 1)
                .Append(") ")
                .AppendLine($"OUTPUT INSERTED.{IdField}")
                .Append(" VALUES (");

            properties.ForEach(prop =>
            {
                if (!prop.Equals(IdField))
                {
                    insertQuery.Append($"@{prop},");
                }
            });

            insertQuery
                .Remove(insertQuery.Length - 1, 1)
                .Append(")");

            return insertQuery.ToString();
        }

        private string GenerateUpdateQuery()
        {
            var updateQuery = new StringBuilder(string.Format("UPDATE {0} SET ", tableName));
            var properties = GenerateListOfProperties(GetProperties());

            properties.ForEach(property =>
            {
                if (!property.Equals(IdField))
                {
                    updateQuery.Append($"{property}=@{property},");
                }
            });

            updateQuery.Remove(updateQuery.Length - 1, 1); //remove last comma
            updateQuery.Append($" WHERE {IdField}=@Id");

            return updateQuery.ToString();
        }

        private static List<string> GenerateListOfProperties(IEnumerable<PropertyInfo> listOfProperties)
        {
            return (
                from prop in listOfProperties 
                let attributes = prop.GetCustomAttributes(typeof(DescriptionAttribute), false)
                where attributes.Length <= 0 || ((attributes[0] as DescriptionAttribute).Description != "ignore")
                select prop.Name).ToList();
        }

        #endregion

        #region IDispose Region
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {

                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
