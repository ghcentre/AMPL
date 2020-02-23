cd /d %~dp0

set WEBDIR=..\Ampl.Web.Mvc.EditorTemplates.Web
set INSTR=.\editor.templates.installation.instructions

rem
rem Styles extending/correcting Bootstrap 3.x
rem
copy /y %WEBDIR%\Content\Classes.css        .\Content\Classes.css
copy /y %WEBDIR%\Content\Fonts.css          .\Content\Fonts.css
copy /y %WEBDIR%\Content\Margins.css        .\Content\Margins.css
copy /y %WEBDIR%\Content\Paddings.css       .\Content\Paddings.css
copy /y %WEBDIR%\Content\BootstrapFixes.css .\Content\BootstrapFixes.css

rem
rem Site style
rem
copy /y %WEBDIR%\Content\Site.css %INSTR%\Content\Site.css.txt

rem
rem Scripts required to run AMPL MVC EditorTemplates
rem 
copy /y %WEBDIR%\Scripts\ampl*    .\Scripts\

rem
rem CLDR data
rem
if not exist .\Scripts\cldr\nul              md .\Scripts\cldr
if not exist .\Scripts\cldr\main\nul         md .\Scripts\cldr\main
if not exist .\Scripts\cldr\supplemental\nul md .\Scripts\cldr\supplemental

xcopy /e /y %WEBDIR%\Scripts\cldr\main\*         .\Scripts\cldr\main\
copy  /y    %WEBDIR%\Scripts\cldr\supplemental\* .\Scripts\cldr\supplemental\

rem
rem DisplayTemplates
rem
del /q .\Views\Shared\DisplayTemplates\*
copy /y %WEBDIR%\Views\Shared\DisplayTemplates\* .\Views\Shared\DisplayTemplates\

rem
rem EditorTemplates
rem
del /q .\Views\Shared\EditorTemplates\*
copy /y %WEBDIR%\Views\Shared\EditorTemplates\* .\Views\Shared\EditorTemplates\

rem
rem Layout template with all sections defined and dependent partials
rem
copy /y %WEBDIR%\Views\Shared\_AlertViewPartial.cshtml	 %INSTR%\Views\Shared\_AlertViewPartial.cshtml.txt
copy /y %WEBDIR%\Views\Shared\_HeadingViewPartial.cshtml %INSTR%\Views\Shared\_HeadingViewPartial.cshtml.txt
copy /y %WEBDIR%\Views\Shared\_Layout.cshtml			 %INSTR%\Views\Shared\_Layout.cshtml.txt
