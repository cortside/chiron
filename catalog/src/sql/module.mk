SQL_SERVER ?= .
SQL_DATABASE ?= .
SQL_USER ?= .
SQL_PASSWORD ?= .

# Path to SQL Server bin.
SQL := mysql --host=${SQL_SERVER} --user=${SQL_USER} --password=${SQL_PASSWORD} ${SQL_DATABASE}

EXTERNALLOGS := $(patsubst sql/external/%.sql, sql/external/%.log, $(sort $(wildcard sql/external/*.sql)))
SCHEMALOGS := $(patsubst sql/schema/%.sql, sql/schema/%.log, $(sort $(sort $(wildcard sql/schema/*.sql))))
TABLELOGS := $(patsubst sql/table/%.sql, sql/table/%.log, $(sort $(sort $(wildcard sql/table/*.sql))))
VIEWLOGS  := $(patsubst sql/view/%.sql, sql/view/%.log, $(sort $(wildcard sql/view/*.sql)))
FUNCTIONLOGS  := $(patsubst sql/function/%.sql, sql/function/%.log, $(sort $(wildcard sql/function/*.sql)))
PROCLOGS  := $(patsubst sql/proc/%.sql, sql/proc/%.log, $(sort $(wildcard sql/proc/*.sql)))
DATALOGS  := $(patsubst sql/data/%.sql, sql/data/%.log, $(sort $(wildcard sql/data/*.sql)))
REPORTLOGS  := $(patsubst sql/report/%.sql, sql/report/%.log, $(sort $(wildcard sql/report/*.sql)))
TRIGGERLOGS  := $(patsubst sql/trigger/%.sql, sql/trigger/%.log, $(sort $(wildcard sql/trigger/*.sql)))
TESTDATALOGS  := $(patsubst sql/testdata/%.sql, sql/testdata/%.log, $(sort $(wildcard sql/testdata/*.sql)))
TYPELOGS	:= $(patsubst sql/type/%.sql, sql/type/%.log, $(wildcard sql/type/*.sql))

.PHONY : db_info clean_db build_db load_data schemas tables views procs debug_db external

db_info:
	@echo SQL_SERVER = $(SQL_SERVER)
	@echo SQL_DATABASE = $(SQL_DATABASE)
	@echo SQL_USER = $(SQL_USER)
	@echo SQL_PASSWORD = $(SQL_PASSWORD)
	@echo
	@echo Usage: db_info clean_db build_db load_data

clean_procs:
	rm -rf $(PROCLOGS)
	
clean_db: clean_procs
	rm -rf $(EXTERNALLOGS)
	rm -rf $(SCHEMALOGS)
	rm -rf $(TABLELOGS)
	rm -rf $(VIEWLOGS)
	rm -rf $(DATALOGS)
	rm -rf $(TESTDATALOGS)
	rm -rf $(TYPELOGS)
	rm -rf $(FUNCTIONLOGS)
	rm -rf $(REPORTLOGS)
	rm -rf $(TRIGGERLOGS)
	
build_db: $(EXTERNALLOGS) $(SCHEMALOGS) $(TABLELOGS) $(VIEWLOGS) $(TYPELOGS) $(FUNCTIONLOGS) $(PROCLOGS) $(TRIGGERLOGS) $(DATALOGS) $(REPORTLOGS)

external: $(EXTERNALLOGS)

schemas: $(SCHEMALOGS)

tables: $(TABLELOGS)

views: $(VIEWLOGS)

types: $(TYPELOGS)

functions: $(FUNCTIONLOGS)

procs: $(PROCLOGS)

data: $(DATALOGS)

report: $(REPORTLOGS)

trigger: $(TRIGGERLOGS)

%.log: %.sql 
	$(SQL) < $< > $@

.DELETE_ON_ERROR:

debug_db:
	@echo EXTERNALLOGS = $(EXTERNALLOGS)
	@echo SCHEMALOGS = $(SCHEMALOGS)
	@echo TABLELOGS = $(TABLELOGS)
	@echo VIEWLOGS = $(VIEWLOGS)
	@echo TYPELOGS = $(TYPELOGS)
	@echo FUNCTIONLOGS = $(FUNCTIONLOGS)
	@echo PROCLOGS = $(PROCLOGS)
	@echo DATALOGS = $(DATALOGS)
	@echo REPORTLOGS = $(REPORTLOGS)
	@echo TESTDATALOGS = $(TESTDATALOGS)
	@echo TRIGGERLOGS = $(TRIGGERLOGS)
	
deleteunittestdata:
	$(SQL) $(SQL_LOGIN) -r -Q "set ANSI_WARNINGS on; set ANSI_PADDING on; set CONCAT_NULL_YIELDS_NULL on; exec spDeleteTestData"

rebuild_db: droptables clean_db build_db load_data initial_testdata test_data

droptables:
	$(SQL) $(SQL_LOGIN)  $(SQL_FLAGS) sql/query/DropAllTables.sql

load_data: db_info build_db $(DATALOGS)

test_data: db_info build_db $(TESTDATALOGS)

initial_testdata:
	$(SQL) $(SQL_LOGIN)  $(SQL_FLAGS) sql/query/initial_testdata.sql
	
dropdb:
	$(SQL) $(SQL_LOGIN)  $(SQL_FLAGS) sql/query/dropdb.sql

backupdb:
	$(SQL) $(SQL_LOGIN)  $(SQL_FLAGS) sql/query/backupdb.sql
	
	
