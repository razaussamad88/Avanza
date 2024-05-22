
cls
write-output	""
write-output	""
write-output	""

write-output	"*** Connecting Avanza Repo Servers..."
write-output	"****************************************************************************************************"
write-output	":::  SVN  :::"
# ping svn1.avanzasolutions.com
TNC -InformationLevel "Detailed" -ComputerName svn1.avanzasolutions.com -Port 91
write-output	""
write-output	""
write-output	":::  GitHub  :::"
# ping git-hub.avanza.pk
TNC -InformationLevel "Detailed" -ComputerName git-hub.avanza.pk -Port 91
write-output	""
write-output	""
write-output	"-----  Completed!!  -----"
write-output	"####################################################################################################"
write-output	""
write-output	""

write-output	""
write-output	""
write-output	"*** Connecting Avanza Jenkins Server..."
write-output	"****************************************************************************************************"
ping jenkins1.avanzasolutions.com
write-output	""
write-output	""
write-output	"-----  Completed!!  -----"
write-output	"####################################################################################################"
write-output	""
write-output	""

write-output	""
write-output	""
write-output	"*** Connecting Avanza Phabricator Server..."
write-output	"****************************************************************************************************"
ping phabricator1.avanzasolutions.com
write-output	""
write-output	""
write-output	"-----  Completed!!  -----"
write-output	"####################################################################################################"
write-output	""
write-output	""

write-output	""
write-output	""
write-output	"*** Connecting Avanza Oracle Servers..."
write-output	"****************************************************************************************************"
write-output	":::  Rdv Notification  :::"
ping 172.16.5.71
write-output	""
write-output	""
write-output	":::  ATG PD Team  :::"
ping 172.16.0.75
write-output	""
write-output	""
write-output	":::  ATG Mobile Team  :::"
ping 172.16.0.48
write-output	""
write-output	""
write-output	"-----  Completed!!  -----"
write-output	"####################################################################################################"
write-output	""
write-output	""

write-output	""
write-output	""
write-output	"*** Connecting Avanza SQL Servers..."
write-output	"****************************************************************************************************"
write-output	":::  ATG-SQL2012  :::"
ping 172.16.0.74
write-output	""
write-output	""
write-output	":::  ATG-SQL2014  :::"
ping 172.16.0.64
write-output	""
write-output	""
write-output	":::  ATG-SQL2017  :::"
ping 172.16.0.94
write-output	""
write-output	""
write-output	"-----  Completed!!  -----"
write-output	"####################################################################################################"
write-output	""
write-output	""

write-output	""
write-output	""
write-output	""
