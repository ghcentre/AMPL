
cd /d %~dp0
set WEBDIR=..\Ampl.Web.Mvc.EditorTemplates.Web

for %%i in (Classes.css Fonts.css Margins.css Paddings.css BootstrapFixes.css Site.css) do copy /y %WEBDIR%\Content\%%i .\Content\
copy /y %WEBDIR%\Scripts\ampl* .\Scripts\

del /q .\Views\Shared\DisplayTemplates\*
copy /y %WEBDIR%\Views\Shared\DisplayTemplates\* .\Views\Shared\DisplayTemplates\

del /q .\Views\Shared\EditorTemplates\*
copy /y %WEBDIR%\Views\Shared\EditorTemplates\* .\Views\Shared\EditorTemplates\

copy /y %WEBDIR%\Views\Shared\_AlertViewPartial.cshtml .\Views\Shared\

for %%i in (_HeadingViewPartial.cshtml _Layout.cshtml) do copy /y %WEBDIR%\Views\Shared\%%i .\editor.templates.installation.instructions\Views\Shared\


pause
