#include "Header.h"

#include "tstring.h"

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

VARIANT_BOOL bool2vbool(bool bTemp)
{
	VARIANT_BOOL vbTemp;

	bTemp == true ? vbTemp = VARIANT_TRUE : vbTemp = VARIANT_FALSE;

	return vbTemp;
}

bool vbool2bool(VARIANT_BOOL vbTemp)
{
	bool bTemp;

	vbTemp == VARIANT_TRUE ? bTemp = true : bTemp = false;

	return bTemp;
}
