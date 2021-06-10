// tstring.h - Wrappers for STL string/stream functionality to use tchars

#include <tchar.h>
#include <string>
#include <ios>
#include <streambuf>
#include <istream>
#include <ostream>
#include <sstream>
#include <fstream>
#include <list>

typedef std::basic_ios<_TCHAR, std::char_traits<_TCHAR> > tios;
typedef std::basic_streambuf<_TCHAR, std::char_traits<_TCHAR> > tstreambuf;
typedef std::basic_istream<_TCHAR, std::char_traits<_TCHAR> > tistream;
typedef std::basic_ostream<_TCHAR, std::char_traits<_TCHAR> > tostream;
typedef std::basic_iostream<_TCHAR, std::char_traits<_TCHAR> > tiostream;
typedef std::basic_stringbuf<_TCHAR, std::char_traits<_TCHAR>, std::allocator<_TCHAR> > tstringbuf;
typedef std::basic_istringstream<_TCHAR, std::char_traits<_TCHAR>, std::allocator<_TCHAR> > tistringstream;
typedef std::basic_ostringstream<_TCHAR, std::char_traits<_TCHAR>, std::allocator<_TCHAR> > tostringstream;
typedef std::basic_stringstream<_TCHAR, std::char_traits<_TCHAR>, std::allocator<_TCHAR> > tstringstream;
typedef std::basic_filebuf<_TCHAR, std::char_traits<_TCHAR> > tfilebuf;
typedef std::basic_ifstream<_TCHAR, std::char_traits<_TCHAR> > tifstream;
typedef std::basic_ofstream<_TCHAR, std::char_traits<_TCHAR> > tofstream;
typedef std::basic_fstream<_TCHAR, std::char_traits<_TCHAR> > tfstream;
typedef std::basic_string<_TCHAR, std::char_traits<_TCHAR>, std::allocator<_TCHAR> > tstring;

typedef std::list<tstring> tstringlist;



