# Secret Santa

## Description
This program generates a [Secret Santa](https://en.wikipedia.org/wiki/Secret_Santa) repartition from a members list.

It has the following functionalities/management rules:
* A member will never gift a present to themselves.
* If a member, Alice, is picked to gift a present to Bob, Bob will not be picked to gift a present to Alice.
* You can specify a custom list of gifting constraints (more information below).
* **If you specify a SMTP server with credentials**, each member can be notified by email to whom they have to gift a present.
* If not, the full results are displayed in console.
* In any case the full results are saved to an output file.
* To prevent the person that uses the program to know who offers to who from reading the full results, the names of those who receive a present can be cyphered with a very basic [Caesar's code](https://en.wikipedia.org/wiki/Caesar_cipher), with a shift of minus one (B becomes A). (more information below) 

## How to use
* Review the configuration in `appsettings.json` and in the different input files. (more information below).
* Start the program with `SecretSanta.Console`.

## Configuration

### The appsettings file
The applicaiton behavior can be configured from the file `appsettings.json`. Here is a description of the following feilds:

| Feild | Description | Example |
| - | - | - |
| `GeneralConfiguration` | The node under which resides the general configuration. | NA |
| `EncypherLocalResult` | A boolean used to set wether the results should be cyphered or not in the display console and in the ouputfile. | `true` |
| `SendResultsByEmail` | A boolean used to set wether the applicaiton must send to each member to whom they have to gift a present. Setting this to true require to have a correct configuration under the `SmtpConfig` node below. | `false` |
| `FileConfiguration` | The node under which resides the input and output files configuration | NA |
| `MembersFilePath` | The path to the file that contains the members list. Will not be used if `SendResultsByEmail` is set to `true`. More information below. | `"./InputOutputFiles/memberList.txt"` |
| `MembersWithEmailFilePath` | The path to the file that contains the members list and their emails. Will not be used if `SendResultsByEmail` is set to `false`. More information below. | `"./InputOutputFiles/memberListWithEmails.csv"` |
| `ConstraintsFilePath` | The path to the file that contains the list of the custom gifting constraints. More information below. | `"./InputOutputFiles/constraints.csv"` |
| `ResultFilePath` | The path to the file that will contain the full result list after the program execution. | `"./InputOutputFiles/output.txt"` |
| `SmtpConfig` | The node under which resides the SMTP configuration to send emails. Will be used if `SendResultsByEmail` is set to `true`. | NA |
| `SmtpClientUsername` | The SMTP client username (email adress) that will be used to connect to the SMTP server and send email on its behalf. | `"name.surname@gmail.com"` |
| `SmtpClientPassword` | The SMTP client password that will be used to connect to the SMTP server. | `"ffhjdzrnlwaabsqp"` |
| `SmtpServerHost` | The SMTP server hostname to connect to with the account above to send emails. | `"smtp.gmail.com"` |
| `SmtpServerPort` | The SMTP server port. | `587` |

### The member lists

Two types of member lists can be specified to the application.
* The one under `MembersFilePath` for when `SendResultsByEmail` is false.
* The one under `MembersWithEmailFilePath` for when `SendResultsByEmail` is true.

#### Members File (Classic text file)
Must contain the name of the members. One per line.

Example: 
```
Alice
Bob
```

#### Members With Email (CSV File)
Must contrain the line of headers and one line per member.

Example: 
```
member,email
Alice,alice@mail.com
Bob,Bob@mail.com
```

### Constraints Files
Must contain the line of headers. **Then can contain no other lines**, or one line per constrait.\
If it does contain lines though, the members listed in there must be written the exact same way as in the member list file (case insensitive).
```
CannotGiftToMemberB,CannotReceiveFromMemberA,IsViceVersa
Alice,Bob,true
Carol,Daniel,false
```
The preceding file would be read as:
* Alice cannot gift a present to Bob
* Bob cannot gift a present to Alice
* Carol cannot gift a present to Daniel

### Caesar's cypher Functionality
If the `EncypherLocalResult` configuration node is set to `true`, the name of the person that receives the present will be cyphered with [Caesar's code](https://en.wikipedia.org/wiki/Caesar_cipher), with a shift of minus one (B becomes A).

Example: Alice become Zkhbd

But to avoid recognizing the cyphered name among the other just by counting the letters, garbage letters can be added in front of, or behid the cyphered name, so every name in the list has the same number of letters.\
Case is also flattened to lowercase, and accents and special charaters are removed.

Example:
| Clear | Cypher |
| - | - |
| Alice | rzkhbdn |
| Bob | xdyanap |

## How to use the SMTP server from Google

In order to send emails with the application, you must connect to an SMTP server, with the config values provided in `appsettings.json`.\
A "quick"-win way of doing it is by using the one from Google, connect with you own Google account and the app will send emails with your adress.

You first need to generate an **App password** in your google account (which unfortunately requires to enamble 2FA). [Here is how to do it](https://support.google.com/accounts/answer/185833?hl=en).

And then you need to fill the `SmtpConfig` section in the `appsettings.json`:
```json
"SmtpConfig": {
	"SmtpClientUsername": "your.name@gmail.com",
	"SmtpClientPassword": "your-app-password",
	"SmtpServerHost": "smtp.gmail.com",
	"SmtpServerPort": 587
}
```