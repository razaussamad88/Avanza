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
Echo === deleting... all user Files
Echo ============================================

FOR /r %%f in (*.user) DO (
	Echo %%f
	del /Q "%%f"
	)
Echo Success!
Echo --------------------------------------------
Echo.
Echo.



Echo ============================================
Echo === deleting... all vssscc Files
Echo ============================================

FOR /r %%f in (*.vssscc) DO (
	Echo %%f
	del /Q "%%f"
	)
Echo Success!
Echo --------------------------------------------
Echo.
Echo.



Echo ============================================
Echo === deleting... all suo Files
Echo ============================================

FOR /r %%f in (*.suo) DO (
	Echo %%f
	del /Q "%%f"
	)
Echo Success!
Echo --------------------------------------------
Echo.
Echo.



Echo ============================================
Echo === deleting... all vspscc Files
Echo ============================================

FOR /r %%f in (*.vspscc) DO (
	Echo %%f
	del /Q "%%f"
	)
Echo Success!
Echo --------------------------------------------
Echo.
Echo.



Echo ============================================
Echo === deleting... all scc Files
Echo ============================================

FOR /r %%f in (*.scc) DO (
	Echo %%f
	del /Q "%%f"
	)
Echo Success!
Echo --------------------------------------------
Echo.
Echo.



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
Echo.
Echo.
