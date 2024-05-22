Rem ==============================================
Rem ---	Delete all BIN, OBJ Folders and VSS
Rem ---	files (*.scc, *.vssscc, *.vspscc)
Rem ---	Created By	:	Muhammad Raza us Samad
Rem ---	Date 		: 	5 July 2013	
Rem ==============================================

@echo off

cls

cd %~dp0


Echo ============================================
Echo === deleting... all Bin Folders
Echo ============================================
FOR /D /r %%G in ("*bin") DO (
	Echo %%G
	RD /S /Q "%%G"
	)
Echo Success!
Echo --------------------------------------------
Echo.
Echo.

Echo ============================================
Echo === deleting... all Obj Folders
Echo ============================================
FOR /D /r %%G in ("*obj") DO (
	Echo %%G
	RD /S /Q "%%G"
	)
Echo Success!
Echo --------------------------------------------


Echo deleting... all VSS (scc)
FOR /r %%G in ("*.scc") DO (
	Echo %%G
	DEL /F /Q "%%G"
	)
Echo Success!
Echo --------------------------------------------

Echo deleting... all VSS (vssscc)
FOR /r %%G in ("*vssscc") DO (
	Echo %%G
	DEL /F /Q "%%G"
	)
Echo Success!
Echo --------------------------------------------

Echo.
Echo.

pause