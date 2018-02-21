FROM microsoft/mssql-server-windows-developer

ENV ACCEPT_EULA Y

# add the scripts to execute to build the db (NOT ideal)
ADD *.sql ./sql/*.sql c:/sql/

# with the service start, run all of the scripts
RUN mkdir c:/MSSQL; \
	start-service MSSQLSERVER; \
	sqlcmd -d master -i c:\\sql\\000-auth.database.sql; \
	sqlcmd -d chiron -i c:\\sql\\000-auth.schema.sql; \
	sqlcmd -d chiron -i c:\\sql\\001-PersistedGrants.table.sql; \
	sqlcmd -d chiron -i c:\\sql\\002-Clients.table.sql; \
	sqlcmd -d chiron -i c:\\sql\\003-IdentityResources.table.sql; \
	sqlcmd -d chiron -i c:\\sql\\004-ApiResources.table.sql; \
	sqlcmd -d chiron -i c:\\sql\\005-ApiClaims.table.sql; \
	sqlcmd -d chiron -i c:\\sql\\006-ApiScopes.table.sql; \
	sqlcmd -d chiron -i c:\\sql\\007-ApiScopeClaims.table.sql; \
	sqlcmd -d chiron -i c:\\sql\\008-ApiSecrets.table.sql; \
	sqlcmd -d chiron -i c:\\sql\\009-IdentityClaims.table.sql; \
	sqlcmd -d chiron -i c:\\sql\\010-ClientClaims.table.sql; \
	sqlcmd -d chiron -i c:\\sql\\011-ClientCorsOrigins.table.sql; \
	sqlcmd -d chiron -i c:\\sql\\012-ClientGrantTypes.table.sql; \
	sqlcmd -d chiron -i c:\\sql\\013-ClientIdPRestrictions.table.sql; \
	sqlcmd -d chiron -i c:\\sql\\014-ClientPostLogoutRedirectUris.table.sql; \
	sqlcmd -d chiron -i c:\\sql\\015-ClientRedirectUris.table.sql; \
	sqlcmd -d chiron -i c:\\sql\\016-ClientScopes.table.sql; \
	sqlcmd -d chiron -i c:\\sql\\017-ClientSecrets.table.sql; \
	sqlcmd -d chiron -i c:\\sql\\018-ClientProperties.table.sql; \
	sqlcmd -d chiron -i c:\\sql\\100-User.table.sql; \
	sqlcmd -d chiron -i c:\\sql\\101-Role.table.sql; \
	sqlcmd -d chiron -i c:\\sql\\102-UserRole.table.sql; \
	sqlcmd -d chiron -i c:\\sql\\103-UserClaim.table.sql; \
	sqlcmd -d chiron -i c:\\sql\\999-Auth-SeedData.data.sql; \
	sqlcmd -d chiron -i c:\\sql\\000-Catalog.schema.sql; \
	sqlcmd -d chiron -i c:\\sql\\001-Item.table.sql; \
	sqlcmd -d chiron -i c:\\sql\\999-Catalog-SeedData.data.sql; \
	stop-service MSSQLSERVER;
