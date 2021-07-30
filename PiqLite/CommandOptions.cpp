#include "CommandOptions.h"

CommandOptions::CommandOptions(int argc, TCHAR* argv[])
{
	TCHAR szArgument[256];
	TCHAR szKeyword[256], szValue[256];
	_InputPcf = true;

	for (int i = 1; i < argc; i++)
	{
		if (argv[i][0] == '/')
		{
			_tcscpy_s(szArgument, &argv[i][1]);

			GetKeywordAndValue(szArgument, szKeyword, szValue);
			LoadCommandLineOption(szKeyword, szValue);
		}
	}




}



int CommandOptions::GetKeywordAndValue(TCHAR* szBuffer, TCHAR* szKeyword, TCHAR* szValue)
{
	TCHAR	buffer[512];
	TCHAR* p;
	int		retval;

	retval = 0;

	_tcscpy_s(szKeyword, 256, _T(""));
	_tcscpy_s(szValue, 256, _T(""));

	_tcscpy_s(buffer, szBuffer);

	TCHAR* pos = NULL;
	p = _tcstok_s(buffer, _T("="), &pos);
	if (p)
	{
		retval = 1;

		_tcscpy_s(szKeyword, 256, p);

		p = _tcstok_s(NULL, _T("="), &pos);
		if (p)
		{
			_tcscpy_s(szValue, 256, p);

			RemoveLeadingWhiteSpace(szKeyword);
			RemoveTrailingWhiteSpace(szKeyword);

			RemoveLeadingWhiteSpace(szValue);
			RemoveTrailingWhiteSpace(szValue);

			retval = 2;
		}
	}

	return retval;
}

void CommandOptions::LoadCommandLineOption(TCHAR* szKeyword, TCHAR* szValue)
{
	

	if (!_tcsicmp(szKeyword, _T("STYLE")))
	{
		_Style = szValue;
	
	}
	else if (!_tcsicmp(szKeyword, _T("MANIFEST")))
	{
		//_tcscpy_s(pcommandlineoptions->szManifest, szValue);
		_manifest = szValue;
	}
	else if (!_tcsicmp(szKeyword, _T("PROJECT")))
	{
		_Project = szValue;
	}
	else if (!_tcsicmp(szKeyword, _T("ISODIR")))
	{
		_IsometricDirectory = szValue;
	}
	else if (!_tcsicmp(szKeyword, _T("PATH")))
	{
		_path = szValue;
	}
	

	
}

void CommandOptions::RemoveLeadingWhiteSpace(TCHAR* s)
{
	unsigned int i, ls;

	ls = _tcslen(s);

	for (i = 0; i < ls; i++)
	{
		if (s[i] != ' ')
		{
			if (i > 0)
			{
				memcpy(&s[0], &s[i], ls - i);
				s[ls - i] = 0;
			}

			break;
		}
	}

	return;
}

void CommandOptions::RemoveTrailingWhiteSpace(TCHAR* s)
{
	unsigned int i, ls;

	ls = _tcslen(s);

	for (i = ls; i > 0; i--)
	{
		if (s[i - 1] == ' ' ||
			s[i - 1] == 10 ||
			s[i - 1] == 13
			)
			s[i - 1] = 0;
		else
			break;
	}

	return;
}

bool CommandOptions::GetInputPCF()
{
	return _InputPcf;
}
tstring CommandOptions::GetStyle()
{
	return _Style;
}

tstring CommandOptions::GetIsoDir()
{
	return _IsometricDirectory;
}
tstring CommandOptions::GetProject()
{
	return _Project;
}

tstring CommandOptions::GetManifest()
{
	return _manifest;
}
tstring CommandOptions::GetPath()
{
	return _path;
}
