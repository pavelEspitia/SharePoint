
param(
	[string]$userFilePath = $(throw "请指定用户XML文件。"),
	[string]$localMachineName,
	[string]$domainName
)


function createUser($server,$fn,$ln,$an,$am){
	$user = $server.Create("User",$an)
	$user.SetPassword("mPec2010")
	$user.Put("Description",$am)
	$user.Put("FullName",$ln+$fn)
	$user.SetInfo()
	write-host $ln $fn $an "，添加完成。"
	return $user
}

function createOU($server, $ou){
	$wOU = $server.Create("OrganizationalUnit","ou="+$ou)
	$wOU.SetInfo()
	return $wOU
}

function createADUser($server,$fn,$ln,$an,$am){
	$user = $server.Create("User","cn="+$an)
	$user.SetPassword("mPec2010")
	$user.Put("Description",$am)
	$user.Put("FullName",$ln+$fn)
	$user.SetInfo()
	write-host $ln $fn $an "，添加完成。"
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

if($domainName){
	[xml]$userFile=get-content $userFilePath
	foreach($user in $userFile.Es.E)
	{
		[ADSI]$server="LDAP://ou="+$user.D+",ou="+$user.R+",ou="+$user.O+",DC="+$domainName+",DC=com"
		if(!$server.Guid){
			[ADSI]$server="LDAP://ou="+$user.R+",ou="+$user.O+",DC="+$domainName+",DC=com"
			if(!$server.Guid){
				[ADSI]$server="LDAP://ou="+$user.O+",DC="+$domainName+",DC=com"
				if(!$server.Guid){
					[ADSI]$server="LDAP://DC="+$domainName+",DC=com"
					$server = createOU $server $user.O
					$server = createOU $server $user.R
					$server = createOU $server $user.D
				}else{
					$server = createOU $server $user.R
					$server = createOU $server $user.D
				}
			}else{
				$server = createOU $server $user.D
			}
		}
		$wUser = createADUser $server $user.Fn $user.LN $user.AN $user.AM
		if($user.M){
			$wUser.Put("Manager",$domainName+"\\"+$user.M)
			$wUser.SetInfo()
		}
	}
}
