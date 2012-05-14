
param(
	[string]$userFilePath = $(throw "请指定用户XML文件。"),
	[string]$localMachineName,
	[string]$domainName,
	[string]$ouName
)


function createUser($server,$fn,$ln,$an,$am){
	$user = $server.Create("User",$an)
	$user.SetPassword("mPec2010")
	$user.Put("Description",$am)
	$user.Put("FullName",$ln+$fn)
	$user.SetInfo()
	write-host $ln $fn "，添加完成。"
	return $user
}

if($localMachineName){
	[ADSI]$server="WinNT://" + $localMachineName
	[xml]$userFile=get-content $userFilePath
	foreach($user in $userFile.Es.E)
	{
		$wUser = createUser $server $user.Fn $user.LN $user.AN $user.AM
	}
}

if($domainName -And $ouName){
	[ADSI]$server="LDAP://ou="+$ouName+",DC="+$domainName+",DC=com"
	[xml]$userFile=get-content $userFilePath
	foreach($user in $userFile.Es.E)
	{
		$wUser = createUser $server $user.Fn $user.LN "cn="+$user.AN $user.AM
		if($user.M){
			$wUser.Put("Manager",$domainName+"\\"+$user.M)
			$wUser.SetInfo()
		}
	}
}
