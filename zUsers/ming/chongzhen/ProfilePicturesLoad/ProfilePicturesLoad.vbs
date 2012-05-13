' This script loads images into an Active Directory schema
' Run under domain administrator privileges
' It assumes images are in a filename format of "[firstname] [lastname].jpg"


' Define variables
' log file name
logFile = "C:\Demos\PeoplePack\PeoplePack_log_file.txt"

' Source dir for images
ImageDir = "C:\Demos\PeoplePack\Images"


' ******************************************************************

Const ForReading = 1

' setup logging
Set objFSO = CreateObject("scripting.filesystemobject")
Set logStream = objFSO.createtextfile(logFile, True)


Set fsHandle = CreateObject("Scripting.FileSystemObject")

' Establish a connection to the AD
set oIADS = GetObject("LDAP://RootDSE")
strDefaultNamingContext = oIADS.Get("defaultnamingcontext")
Set adodbConn = CreateObject("ADODB.Connection")
adodbConn.Provider = "ADsDSOObject"
adodbConn.Open "ADs Provider"
Set cLDAPCommand  = CreateObject("ADODB.Command")
cLDAPCommand.ActiveConnection = adodbConn
Set objADRecordSet = CreateObject("ADODB.Recordset")

c = 0 ' Count how many images were loaded

'Look in the image dir to find all the images
For Each tImage In fsHandle.GetFolder(ImageDir).Files
    tName = tImage.Name
    iSize = tImage.Size
	sFileExt = Right(tName,4)
    
    ' Skip if it's not a JPG or bigger than 100k  (max size for AD is 100k)
    If sFileExt = ".jpg" AND iSize < 102400 Then
 
	
	    'Convert filename to persons name, ignoring extension 
	    tName = Left(tName, Len(tName)-4)
	    strLDAPQuery = "<LDAP://" & strDefaultNamingContext & _
	    	">;(&(objectClass=person)(name=" &  tName & _
	    	"));name,adspath;subtree"
	    
	    ' Execute the query and get a result set
	    cLDAPCommand.CommandText = strLDAPQuery
	    Set oResultSet = cLDAPCommand.Execute
	    If oResultSet.RecordCount = 0 Then
	      logStream.writeline "******  Did not find AD account for: " & tName
	    Else
	
	      Set oUserAccount = GetObject(oResultSet("adspath"))
	      oUserAccount.Put "thumbnailPhoto", ReadFileBytes(tImage.Path)
	      oUserAccount.SetInfo
	      logStream.writeline "Write image for " & tName
	      c=c+1

	    End If

   End If  ' end checking for JPGs
 

Next

logStream.writeline "Wrote " & x & " images to AD."
logStream.Close





'Read in the file and return bytecode
Function ReadFileBytes(strFullFilePath)
    
    Dim binFH
    
    'create a binary handle to the file
    Set binFH = CreateObject("ADODB.Stream")
    binFH.Type = 1
    binFH.Open
    binFH.LoadFromFile (strFullFilePath)
    ReadFileBytes = binFH.Read
    
End Function





