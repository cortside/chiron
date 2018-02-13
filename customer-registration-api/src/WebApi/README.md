# SavedSearch

TODO:
- need JWT library
http://stackoverflow.com/questions/30546542/token-based-authentication-in-asp-net-5-vnext-refreshed
http://error.news/question/2160472/dnx-core-50-jwtsecuritytokenhandler-ldquoidx10640-algorithm-is-not-supported-39httpwwww3org200104xmldsig-morehmac-sha25639rdquo/

/*
const string issuer = "issuer";
const string audience = "audience";
byte[] keyForHmacSha256 = new byte[32];
new Random().NextBytes(keyForHmacSha256);
var claims = new List<Claim> { new Claim("deviceId", "12") };
var now = DateTime.UtcNow;
var expires = now.AddHours(1);
var signingCredentials = new SigningCredentials(
new SymmetricSecurityKey(keyForHmacSha256),
SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha256Digest);
var token = new jwtSecurityToken(issuer, audience, claims, now, expires, signingCredentials);
return _tokenHandler.WriteToken(token);
*/

http://odetocode.com/blogs/scott/archive/2015/01/15/using-json-web-tokens-with-katana-and-webapi.aspx
^^^ this one I think


to run:
	init.bat
	run.bat
	dnx web

In browser go to:
	http://localhost:5000/api/savedsearch/searches

Notes:
	Every table:
		Has an audit stamp = (CreatedDate, CreatedByUserId, LastModifiedDate, LastModifiedByUserId)
		Has a primary key id which is the TableName + Id (TableNameId)
		Any foreign keys are as follows - FK_TableName_ColumnName
		
	Adopt common styling for names
		Pascal casing - tables, table entities, schema's, C# objects
		Camel casing - Everything in json
		
	Use Visual Studio 2015 for all code, editing, debugging, etc.
	
TODO	
	*This all needs to go into a new schema called SavedSearch
		Tables (SavedSearch database/schema):
		SearchCriteria
		- SearchCriteriaId
		- SearchCriteriaKey (this is an MD5SUM of the values)
		- <audit stamp>

		SearchCriteriaValue
		- SearchCriteriaValueId
		- SearchCriteriaId (fk)
		- Key
		- Value
		- <audit stamp>

		SavedSearch
		- SavedSearchId
		- UserId
		- SearchCriteriaId (fk)
		- SendAlert
		- LastIndexId
		- LastSent
		- <audit stamp>
		
		SearchCriteria is immutible with a hash
			Never an update or delete from SearchCriteria
			Stored as key/value pairs as we get them from the API
		SavedSearch holds the reference to a set of criterea
			fk to the criteria
			bidder id (makes it specific to a bidder)
			whther they're getting an alert
			info about the send alerts job (when last sent and SOLR index id)
	
	
	Create APIs
		api/savedsearch/*
	
	Update the json webtoken, as all the methods/apis are expected to be authenticated
	
	Use config.json to store the connection string (see in startup.js where the config/json is referenced/used already. )
	Write nodeunit tests as done before. Create a folder in this project.
