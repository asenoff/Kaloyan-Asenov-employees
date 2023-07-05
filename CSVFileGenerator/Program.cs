using System.Text;

// ten years past
const int MAX_DAYS_AGO = 3653;
const int MAX_EMPLOYEE_ID = 100;
const int MAX_PROJECT_ID = 1000;
const int NULL_INCIDENCE = 20;
const int NUMBER_OF_ROWS = 10000;


Random random = new Random();
StringBuilder fileContents = new StringBuilder();
fileContents.AppendLine("EmpID,ProjectID,DateFrom,DateTo");
for(int i = 0; i < NUMBER_OF_ROWS; i++)
{
    var empID = random.Next(1, MAX_EMPLOYEE_ID);
    var projectID = random.Next(1, MAX_PROJECT_ID);
    var floorForToDate = random.Next(1, MAX_DAYS_AGO);
    var fromDate = DateTime.Now.AddDays(-floorForToDate).ToString("yyyy-MM-dd");
    int randomNumberNullGen = random.Next(1, NULL_INCIDENCE);
    var toDate = randomNumberNullGen == NULL_INCIDENCE - 1 ? "NULL" : DateTime.Now.AddDays(-random.Next(1, floorForToDate)).ToString("yyyy-MM-dd");

    fileContents.AppendLine($"{empID},{projectID},{fromDate},{toDate}");
}

string filePath = "../../../sampleFile.csv"; // The file path where you want to save the file
File.WriteAllText(filePath, fileContents.ToString());

