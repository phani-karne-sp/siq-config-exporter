12/2016 5.0.0.0
Released

02/15/2017 5.0.0.1
- Added fix for problem where SecurityIQ dlls were only found if located on c: or e:

03/20/2017 5.0.0.2
- Updated the 'apps' query to fix a problem where the ECname was always null.  Also renamed the field from ECname to PermColl
- Updated the 'install_service' query to fix problem where each core service displayed 1 time per installed_server

03/31/2017 5.0.0.3
- Wrapped the password in single quotes when putting in connection string (to handle semi-colons)
- Escaped single-quote in password provided by user

04/02/2017 5.0.0.4
- Refactored the database fetches to include better error handling

06/27/2017 5.1.0.0
- Added option to use the nHibernate connections string credentials or use the user supplied credentials
