#SetupUser.ps1-v3(forked)

#---------------------------------------------------------------------------------
# Parameters
#---------------------------------------------------------------------------------

param(
    [string]$siteUrl = $(throw "please specify the url of a SharePoint site to open!"),
    [string]$mySiteHostUrl = $(throw "please specify the url of MySite Host!"),
    [System.Collections.Specialized.StringDictionary]$data = $(throw "please specify a data dictionary!")
)

#---------------------------------------------------------------------------------
# Default Values
#---------------------------------------------------------------------------------

$userFullName = "Unknown";
$spNotFoundMsg = "Unable to connect to SharePoint.  Please verify that the site '$siteUrl' is hosted on the local machine.";

#---------------------------------------------------------------------------------
# Load Assemblies
#---------------------------------------------------------------------------------

if ([Reflection.Assembly]::LoadWithPartialName("Microsoft.SharePoint") -eq $null)		{ throw $spNotFoundMsg; }
if ([Reflection.Assembly]::LoadWithPartialName("Microsoft.Office.Server") -eq $null)	{ throw $spNotFoundMsg; }
[void] [Reflection.Assembly]::LoadWithPartialName("System.Web")
. .\SPUtils.ps1


#---------------------------------------------------------------------------------
# Process the passed in data
#---------------------------------------------------------------------------------

""
([System.String]::Empty.PadRight(20, "-"))

$userFullName = ($data["LastName"] + ([string]", ") + $data["FirstName"]);
([string]"Operating against user: $userFullName.  With login name: " + $data["AccountName"])


#-----------------------------------------------------
# Get User Profile
#-----------------------------------------------------

[Microsoft.Office.Server.UserProfiles.UserProfileManager]$upm = $null;

try
{
	$upm = GetProfileManager -url $siteUrl
}
catch [System.Exception]
{
	throw $spNotFoundMsg;
}

try
{
	#-----------------------------------------------------
	# Create User Profile if it does not exist
	#-----------------------------------------------------

	if ($upm.UserExists($data["AccountName"]))
	{
		"User Profile exists for '$userFullName'.  Fetching Profile..."
		$up = $upm.GetUserProfile($data["AccountName"]);
	}
	else
	{
		"User Profile does not exist for '$userFullName'.  Creating Profile..."
		$up = $upm.CreateUserProfile($data["AccountName"]);
	}


	#-----------------------------------------------------
	# Get Personal Site
	#-----------------------------------------------------

	if ($up.PersonalSite -eq $null)
	{
		"Personal Site does not exist for '$userFullName'."
        
        if($data["CreateMySite"] -eq "True")
        {
            "Creating Personal Site for '$userFullName'..." 
            $up.CreatePersonalSite();
            ([string]"Personal Site Url for '$userFullName':" + [string]$up.PersonalSite.Url)
        }
        else
        {
            "Skipping Personal Site creation for '$userFullName'."
        }
	}
	else
	{
        "Personal Site exists for '$userFullName'."
	}

	

	#-----------------------------------------------------
	# Upload User Profile Picture
	#-----------------------------------------------------

	""
	(([string]"Uploading portrait for '$userFullName': ") + ([string]$data["picture"]) + ([string]" to: " + [string]$mySiteHostUrl + [string]"/User Photos/"))

	$verboseLog = New-Object System.Text.StringBuilder;
	[Microsoft.SharePoint.SPFile]$file = UploadSPFile -verbose $verboseLog -url ($mySiteHostUrl) -libraryName "User Photos" -filePath (Resolve-Path $data["picture"])
	$verboseLog.ToString();

	#-----------------------------------------------------
	# Set User Profile Properties
	#-----------------------------------------------------

	"Setting Profile Properties for '$userFullName':"

	$picturePropertyName = GetProfilePropertyName -UserProfileManager $upm -PropertyName "PictureUrl";

	if (-not [System.String]::IsNullOrEmpty($picturePropertyName))
	{
		$PortraitUrl = CombineUrls -baseUrl $file.Web.Url -relUrl $file.ServerRelativeUrl;
		([string]'Setting user portrait Url to "' + [string]$PortraitUrl + [string]'"...')
		$up.get_Item($picturePropertyName).Value = $PortraitUrl;
	}

	foreach($de in $data)
	{
		if (($de.Key -ne "accountname") -and ($de.Key -ne "picture"))
		{
			[string]$propName = GetProfilePropertyName -UserProfileManager $upm -PropertyName $de.Key;
			if (-not [System.String]::IsNullOrEmpty($propName))
			{
				([string]"Setting " + $de.Key + [string]"...");
				if(($de.Key -ne "askmeabout") -and ($de.Key -ne "pastprojects") -and ($de.Key -ne "interests")) {
					$up.get_Item($propName).Value = ([string]$de.Value);
				} else {
					$a=$up.get_Item($propName)
					$a.Clear()
					$values = ([string]$de.Value).split(",")
					foreach($val in $values) {
						([string]"    Adding Value: " + [string]$val);
						[void]$a.Add($val);
					}
				}
			}
		}
	}

	"Committing Changes for '$userFullName'...";
	$up.Commit();
	"Update for '$userFullName' Complete."
}
catch [System.Exception]
{
    write-warning ([string]"Unable to provision user '$userFullName'.  Skipping User.  See full log for details.\n\tError:" + $error[0].ToString());
}
