using Supabase;
using Supabase.Gotrue;
using Client = Supabase.Client;

namespace ProsperaServices.Supabase;

public class SupabaseService(IConfiguration configuration)
{
    public Client Client { get; } = new(
        configuration["Supabase:Url"]!,
        configuration["Supabase:Key"]
    );
}