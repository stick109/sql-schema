# Sql Schema Comparer for Microsoft Sql Server

[![Build](https://github.com/stick109/sql-schema/actions/workflows/build.yml/badge.svg)](https://github.com/stick109/sql-schema/actions/workflows/build.yml)

[![Publish](https://github.com/stick109/sql-schema/actions/workflows/release.yml/badge.svg)](https://github.com/stick109/sql-schema/actions/workflows/release.yml)

This tool supports three commands: **extract**, **compare**, and **help**. 

**Extract** command allows you to extract schema from given database instance into json file. This can be helpful in a number of scenarios. For example, you can commit result of the extract command to your source control to track changes in your database schema if you don't have better way of tracking schema changes.

**Compare** command allows you to compare two database schemas serialized to json files.

All command line options are explained with **help** command. You can run `ssc help` to get top level help, or `ssc help extract` or `ssc help compare` to get help on the particular command.

## Extract command arguments

| Parameter | Required/Default | Description                  |
|-----------|------------------|------------------------------|
| -s        | Required.        | Server address.              |
| -d        | Required.        | Database name.               |
| -v        | (Default: false) | Verbose.                     |
| --debug   | (Default: false) | Even more verbose.           |
| --help    |                  | Display this help screen.    |
| --version |                  | Display version information. |

  Example: `ssc extract -s localhost -d mydb > mydb.json`

## Compare command arguments

| Parameter | Required/Default | Description                                                           |
|-----------|------------------|-----------------------------------------------------------------------|
| -s        | Required.        | Source schema.                                                        |
| -t        | Required.        | Target schema.                                                        |
| -d        | (Default: false) | Show all properties of missing tables, including columns and indexes. |
| -v        | (Default: false) | Verbose.                                                              |
| --debug   | (Default: false) | Even more verbose.                                                    |
| --help    |                 | Display this help screen.                                             |
| --version |                 | Display version information.                                          |

  Example: `ssc compare -s source.json -t target.json > comparison.json`
