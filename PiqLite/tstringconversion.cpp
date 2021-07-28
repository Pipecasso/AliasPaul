#include "Header.h"
#include "tstring.h"
#include "tstringconversion.h"

tstring	bstr2tstring(BSTR bstr)
{
	tstring strTemp;
	_bstr_t bstrTemp;

	if (bstr)
	{
		bstrTemp = bstr;
		strTemp = bstrTemp;
	}
	else
	{
		strTemp = _T("");
	}

	return strTemp;
}

BSTR tstring2bstr(tstring str)
{
	if (str.length() > 0)
	{
		_bstr_t bstrTemp;

		bstrTemp = str.c_str();

		return bstrTemp.copy();
	}
	else
	{
		return SysAllocString(_T(""));
	}
}
