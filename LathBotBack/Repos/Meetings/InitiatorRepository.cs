using LathBotBack.Base;
using LathBotBack.Models.Meetings;
using LathBotBack.Services;
using System;
using System.Data.SqlClient;

namespace LathBotBack.Repos.Meetings
{
	public class InitiatorRepository : RepositoryBase
	{
		public InitiatorRepository(string connectionString) : base(connectionString) { }

		public bool Create(ref Initiator entity)
		{
			bool result = false;

			try
			{
				DbCommand.CommandText = "INSERT INTO meetings_Initiator (Name) OUTPUT INSERTED.Id VALUES (@name);";
				DbCommand.Parameters.Clear();
				DbCommand.Parameters.AddWithValue("name", entity.Name);
				DbConnection.Open();
				entity.Id = (int)DbCommand.ExecuteScalar();
				DbConnection.Close();

				result = true;
			}
			catch (Exception e)
			{
				SystemService.Instance.Logger.Log(e.Message);
			}
			finally
			{
				if (DbConnection.State == System.Data.ConnectionState.Open)
				{
					DbConnection.Close();
				}
			}

			return result;
		}

		public bool Read(int id, out Initiator entity)
		{
			bool result = false;
			entity = null;

			try
			{
				DbCommand.CommandText = "SELECT * FROM meetings_Initiator WHERE Id = @id;";
				DbCommand.Parameters.Clear();
				DbCommand.Parameters.AddWithValue("id", id);
				DbConnection.Open();
				using SqlDataReader reader = DbCommand.ExecuteReader();
				reader.Read();
				entity = new Initiator
				{
					Id = id,
					Name = (string)reader["Name"]
				};
				DbConnection.Close();

				result = true;
			}
			catch (Exception e)
			{
				SystemService.Instance.Logger.Log(e.Message);
			}
			finally
			{
				if (DbConnection.State == System.Data.ConnectionState.Open)
				{
					DbConnection.Close();
				}
			}

			return result;
		}

		public bool Update(Initiator entity)
		{
			bool result = false;

			try
			{
				DbCommand.CommandText = "UPDATE meetings_Initiator SET Name = @name WHERE Id = @id;";
				DbCommand.Parameters.Clear();
				DbCommand.Parameters.AddWithValue("name", entity.Name);
				DbCommand.Parameters.AddWithValue("id", entity.Id);
				DbConnection.Open();
				DbCommand.ExecuteNonQuery();
				DbConnection.Close();

				result = true;
			}
			catch (Exception e)
			{
				SystemService.Instance.Logger.Log(e.Message);
			}
			finally
			{
				if (DbConnection.State == System.Data.ConnectionState.Open)
				{
					DbConnection.Close();
				}
			}

			return result;
		}

		public bool Delete(int id)
		{
			bool result = false;

			try
			{
				DbCommand.CommandText = "DELETE FROM meetings_Initiator WHERE Id = @id;";
				DbCommand.Parameters.Clear();
				DbCommand.Parameters.AddWithValue("id", id);
				DbConnection.Open();
				DbCommand.ExecuteNonQuery();
				DbConnection.Close();

				result = true;
			}
			catch (Exception e)
			{
				SystemService.Instance.Logger.Log(e.Message);
			}
			finally
			{
				if (DbConnection.State == System.Data.ConnectionState.Open)
				{
					DbConnection.Close();
				}
			}

			return result;
		}
	}
}
