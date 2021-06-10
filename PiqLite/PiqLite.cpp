// PiqLite.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include "Header.h"
#include "PiqLiter.h"



std::list<fsystem::path> GetInputFiles(fsystem::path & p)
{
	std::list<fsystem::path> InputFiles;
	if (fsystem::exists(p) && fsystem::is_directory(p))
	{
		
		for (const auto& entry : fsystem::recursive_directory_iterator(p))
		{

			if (fsystem::is_regular_file(entry.status()))
			{
				if (entry.path().extension() == ".idf" || entry.path().extension() == ".IDF" || entry.path().extension() == ".pcf" || entry.path().extension() == ".PCF") 
				{
					InputFiles.push_back(entry.path());
				}
			}
		}
	}
	return InputFiles;
}

int main(int argc, char* argv[])
{
	fsystem::path p(argv[1]);
	std::list<fsystem::path> InputFiles = GetInputFiles(p);
	fsystem::path m(argv[2]);

	::CoInitialize(0);
	CPiqLiter pliter(InputFiles, m);
	pliter.Go();
	::CoUninitialize();
}





// Run program: Ctrl + F5 or Debug > Start Without Debugging menu
// Debug program: F5 or Debug > Start Debugging menu

// Tips for Getting Started: 
//   1. Use the Solution Explorer window to add/manage files
//   2. Use the Team Explorer window to connect to source control
//   3. Use the Output window to see build output and other messages
//   4. Use the Error List window to view errors
//   5. Go to Project > Add New Item to create new code files, or Project > Add Existing Item to add existing code files to the project
//   6. In the future, to open this project again, go to File > Open > Project and select the .sln file
