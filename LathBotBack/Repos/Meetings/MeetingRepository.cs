using LathBotBack.Base;
using LathBotBack.Models.Meetings;
using LathBotBack.Services;
using System;
using System.Data.SqlClient;

namespace LathBotBack.Repos.Meetings
{
	public class MeetingRepository : RepositoryBase
	{
		public MeetingRepository(string connectionString) : base(connectionString) { }

		public bool GetLastMeeting(out Meeting entity)
		{
			bool result = false;
			entity = null;

			try
			{
				DbCommand.CommandText = "SELECT * FROM meetings_Meeting WHERE Id = (SELECT MAX(Id) FROM meetings_Meeting);";
				DbCommand.Parameters.Clear();
				DbConnection.Open();
				using SqlDataReader reader = DbCommand.ExecuteReader();
				reader.Read();
				entity = new Meeting()
				{
					Id = (int)reader["Id"],
					Number = (int)reader["Number"]
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

		public bool Create(ref Meeting entity)
		{
			bool result = false;

			try
			{
				DbCommand.CommandText = "INSERT INTO meetings_Meeting (Number) OUTPUT INSERTED.Id VALUES (@num);";
				DbCommand.Parameters.Clear();
				DbCommand.Parameters.AddWithValue("num", entity.Number);
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

		public bool Read(int id, out Meeting entity)
		{
			bool result = false;
			entity = null;

			try
			{
				DbCommand.CommandText = "SELECT * FROM meetings_Meeting WHERE Id = @id;";
				DbCommand.Parameters.Clear();
				DbCommand.Parameters.AddWithValue("id", id);
				DbConnection.Open();
				using SqlDataReader reader = DbCommand.ExecuteReader();
				reader.Read();
				entity = new Meeting
				{
					Id = id,
					Number = (int)reader["Number"]
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

		public bool Update(Meeting entity)
		{
			bool result = false;

			try
			{
				DbCommand.CommandText = "UPDATE meetings_Meeting SET Number = @num WHERE Id = @id;";
				DbCommand.Parameters.Clear();
				DbCommand.Parameters.AddWithValue("num", entity.Number);
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
				DbCommand.CommandText = "DELETE FROM meetings_Meeting WHERE Id = @id;";
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
