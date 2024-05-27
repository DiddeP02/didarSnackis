using System.Text.Json;

namespace SnackisUppgift.DAL
{
	public class SubjectManagerAPI
	{
		private static Uri BaseAddress = new Uri("https://localhost:44368/");
		public static async Task<Models.Subject> GetSubject(int id)
		{
			Models.Subject subjects = new();
			using (var client = new HttpClient())
			{
				client.BaseAddress = BaseAddress;
				HttpResponseMessage response = await client.GetAsync("api/Subject/" + id);
				if (response.IsSuccessStatusCode)
				{
					string responseString = await response.Content.ReadAsStringAsync();
					subjects = JsonSerializer.Deserialize<Models.Subject>(responseString);
				}
				return subjects;

			}
		}
		public static async Task<List<Models.Subject>> GetAllSubjects()
		{
			List<Models.Subject> subjects = new();
			using (var client = new HttpClient())
			{
				client.BaseAddress = BaseAddress;
				HttpResponseMessage response = await client.GetAsync("api/Subject/");
				if (response.IsSuccessStatusCode)
				{
					string responseString = await response.Content.ReadAsStringAsync();
					subjects = JsonSerializer.Deserialize<List<Models.Subject>>(responseString);
				}
				return subjects;

			}
		}
		public static async Task DeleteSubject(int id)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = BaseAddress;
				HttpResponseMessage response = await client.DeleteAsync("api/Subject/" + id);
			}
		}
		public static async Task SaveSubject(Models.Subject subject)
		{
			var prod = (await GetAllSubjects()).Where(p => p.Id == subject.Id).FirstOrDefault();
			using (var client = new HttpClient())
			{
				client.BaseAddress = BaseAddress;
				var json = JsonSerializer.Serialize(subject);
				StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

				// If the subject exists, update the subject using PutAsync method
				if (prod != null)
				{
					HttpResponseMessage response = await client.PutAsync("api/Subject/" + subject.Id, httpContent);
				}
				// If the subject doesn't exist, create a new subject using PostAsync method
				else
				{
					HttpResponseMessage response = await client.PostAsync("api/Subject/", httpContent);
				}
			}
		}

		public static async Task UpdateSubject(Models.Subject subject)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = BaseAddress;

				// Convert subject object to JSON
				var json = JsonSerializer.Serialize(subject);
				StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

				// Send a PUT request to your API endpoint
				HttpResponseMessage response = await client.PutAsync("api/Subject/" + subject.Id, httpContent);
			}
		}


	}
}
