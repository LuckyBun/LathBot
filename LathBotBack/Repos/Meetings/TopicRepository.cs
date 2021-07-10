using LathBotBack.Base;
using LathBotBack.Models.Meetings;
using LathBotBack.Services;
using System;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace LathBotBack.Repos.Meetings
{
	public class TopicRepository : RepositoryBase
	{
		public TopicRepository(string connectionString) : base(connectionString) { }

		public bool Create(ref Topic entity)
		{
			bool result = false;

			try
			{
				DbCommand.CommandText = "INSERT INTO meetings_Topic (Meeting, Name, Text, Initiator) OUTPUT INSERTED.Id VALUES (@meeting, @name, @text, @init);";
				DbCommand.Parameters.Clear();
				DbCommand.Parameters.AddWithValue("meeting", entity.Meeting);
				DbCommand.Parameters.AddWithValue("name", entity.Name);
				DbCommand.Parameters.AddWithValue("text", entity.Text);
				DbCommand.Parameters.AddWithValue("init", entity.Initiator);
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

		public bool Read(int id, out Topic entity)
		{
			bool result = false;
			entity = null;

			try
			{
				DbCommand.CommandText = "SELECT * FROM meetings_Topic WHERE Id = @id;";
				DbCommand.Parameters.Clear();
				DbConnection.Open();
				using SqlDataReader reader = DbCommand.ExecuteReader();
				reader.Read();
				entity = new Topic
				{
					Id = id,
					Meeting = (int)reader["Meeting"],
					Name = (string)reader["Name"],
					Text = (string)reader["Text"],
					Initiator = (int)reader["Initiator"]
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

		public bool Update(Topic entity)
		{
			bool result = false;

			try
			{
				DbCommand.CommandText = "UPDATE meetings_Topic SET Name = @name, Meeting = @meeting, Text = @text, Initiator = @init WHERE Id = @id;";
				DbCommand.Parameters.Clear();
				DbCommand.Parameters.AddWithValue("name", entity.Name);
				DbCommand.Parameters.AddWithValue("meeting", entity.Meeting);
				DbCommand.Parameters.AddWithValue("text", entity.Text);
				DbCommand.Parameters.AddWithValue("init", entity.Initiator);
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
				DbCommand.CommandText = "DELETE FROM meetings_Topic WHERE Id = @id;";
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
