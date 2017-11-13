using System.Web;
using System.Web.Optimization;

namespace AiVacina
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/jquery-ui.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/jqueryUi").Include(
                      "~/Content/jquery-ui.min.css",
                      "~/Content/jquery-ui.structure.min.css",
                      "~/Content/jquery-ui.theme.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/inputmask").Include(
            //    "~/Scripts/jquery.mask.js",
                "~/Scripts/jquery.mask.min.js",
                "~/Scripts/jquery-ui-1.12.1.min.js",
                "~/Scripts/jquery-ui.min.js",
                "~/Scripts/Custom/Script.js"));

            bundles.Add(new ScriptBundle("~/bundles/listavacinas").Include(
                "~/Scripts/Custom/ListaVacinas.js"));

            bundles.Add(new ScriptBundle("~/bundles/pacientes").Include(
                "~/Scripts/Custom/PacienteAgenda.js"));

            bundles.Add(new ScriptBundle("~/bundles/cadastrarcarteira").Include(
                "~/Scripts/Custom/CadastrarCarteira.js"));

            bundles.Add(new ScriptBundle("~/bundles/adicionapostos").Include(
                "~/Scripts/Custom/AdicionaPostos.js"));

            bundles.Add(new ScriptBundle("~/bundles/agenda").Include(
                "~/Scripts/Custom/Agenda.js"));

            bundles.Add(new ScriptBundle("~/bundles/postoagenda").Include(
                "~/Scripts/Custom/PostoAgenda.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/pacienteagendamento").Include(
                "~/Scripts/Custom/PacienteAgendamento.js"));

            bundles.Add(new ScriptBundle("~/bundles/postocadastrarvacina").Include(
                "~/Scripts/Custom/PostoCadastrarVacina.js"));

            bundles.Add(new ScriptBundle("~/bundles/partialpostoagendamento").Include(
                "~/Scripts/Custom/PartialPostoAgendamento.js"));

            bundles.Add(new StyleBundle("~/Content/pacienteIicio").Include(
                      "~/Content/Custom/PacienteInicio.css"));

            bundles.Add(new StyleBundle("~/Content/postovacinas").Include(
                      "~/Content/Custom/PostoVacinas.css"));

            bundles.Add(new ScriptBundle("~/bundles/pacienteIicio").Include(
                "~/Scripts/Custom/PacienteInicio.js"));

            bundles.Add(new ScriptBundle("~/bundles/postocarteirapaciente").Include(
                "~/Scripts/Custom/PostoCarteiraPaciente.js"));

            bundles.Add(new StyleBundle("~/Content/layout").Include(
                      "~/Content/Custom/Layout.css"));

            bundles.Add(new StyleBundle("~/Content/listasPostos").Include(
                      "~/Content/Custom/ListaPostos.css"));
        }
    }
}
