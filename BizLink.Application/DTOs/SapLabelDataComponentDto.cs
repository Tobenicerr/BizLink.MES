using BizLink.MES.Application.Mappings;
using BizLink.MES.Domain.Attributes;
using BizLink.MES.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.DTOs
{
    public class SapLabelDataComponentDto : IMapFrom<SapLabelDataComponent>
    {
        public string? TDOBNAME { get; set; }

        private string? _materialCode;

        public string? MATNR
        {
            get
            {
                return _materialCode;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _materialCode = value;
                }
                else
                {
                    _materialCode = value.TrimStart('0');
                }
            }
        }

        public string? WERKS { get; set; }


        public string? ETFORM { get; set; }


        public string? ETNR { get; set; }

        public string? SPRSL { get; set; }

        public string? ETFORMZ { get; set; }

        public string? ANZETP { get; set; }

        public string? ANZET { get; set; }

        public string? MULTITEXT { get; set; }

        public string? ZZFUNK { get; set; }

        public string? INTERNE { get; set; }

        public string? ARBGNR { get; set; }

        public string? AINDEX { get; set; }

        public string? SAPAUF { get; set; }

        public string? AVO { get; set; }

        public string? DATUM1 { get; set; }

        public string? DATUM2 { get; set; }

        public string? LIEFL { get; set; }

        public string? SACHNR { get; set; }

        public string? SACHNRK { get; set; }

        public string? MEINS { get; set; }

        public string? TYPBEZ01 { get; set; }

        public string? TYPBEZ02 { get; set; }

        public string? TYPBEZ03 { get; set; }

        public string? TYPBEZ04 { get; set; }

        public string? TYPBEZ05 { get; set; }

        public string? TYPBEZ06 { get; set; }

        public string? TYPBEZ07 { get; set; }

        public string? TYPBEZ08 { get; set; }

        public string? TYPBEZ09 { get; set; }

        public string? TYPBEZ10 { get; set; }

        public string? TYPBEK01 { get; set; }

        public string? TYPBEK02 { get; set; }

        public string? TYPBEK03 { get; set; }

        public string? TYPBEK04 { get; set; }

        public string? TYPBEK05 { get; set; }

        public string? TYPBEK06 { get; set; }

        public string? TYPBEK07 { get; set; }

        public string? TYPBEK08 { get; set; }

        public string? TYPBEK09 { get; set; }

        public string? TYPBEK10 { get; set; }

        public string? FARBE { get; set; }

        public string? FUNK { get; set; }

        public string? VDENR { get; set; }

        public string? TEXT01 { get; set; }

        public string? TEXT02 { get; set; }

        public string? TEXT03 { get; set; }

        public string? TEXT04 { get; set; }

        public string? TEXT05 { get; set; }

        public string? TEXT06 { get; set; }

        public string? TEXT07 { get; set; }

        public string? TEXT08 { get; set; }

        public string? TEXT09 { get; set; }

        public string? TEXT10 { get; set; }

        public string? TEXT11 { get; set; }

        public string? TEXT12 { get; set; }

        public string? TEXT13 { get; set; }

        public string? TEXT14 { get; set; }

        public string? TEXT15 { get; set; }

        public string? TEXT16 { get; set; }

        public string? TEXT17 { get; set; }

        public string? TEXT18 { get; set; }

        public string? TEXT19 { get; set; }

        public string? TEXT20 { get; set; }

        public string? TEXT21 { get; set; }

        public string? TEXT22 { get; set; }

        public string? TEXT23 { get; set; }

        public string? TEXT24 { get; set; }

        public string? TEXT25 { get; set; }

        public string? LAENGE01 { get; set; }

        public string? FAKTOR01 { get; set; }

        public string? TROMNR01 { get; set; }

        public string? TROMTP01 { get; set; }

        public string? MARK_A01 { get; set; }

        public string? MARK_E01 { get; set; }

        public string? QKENNZ01 { get; set; }

        public string? LAENNR01 { get; set; }

        public string? KABELN01 { get; set; }

        public string? GEWEINH { get; set; }

        public string? BRUTTO01 { get; set; }

        public string? TARAGE01 { get; set; }

        public string? KALWO { get; set; }

        public string? JAHR { get; set; }

        public string? ZZURSP01 { get; set; }

        public string? LAYOUT { get; set; }

        public string? DATUM3 { get; set; }

        public string? UMS01 { get; set; }

        public string? UMS02 { get; set; }

        public string? UMS03 { get; set; }

        public string? UMS04 { get; set; }

        public string? UMS05 { get; set; }

        public string? UMS06 { get; set; }

        public string? UMS07 { get; set; }

        public string? UMS08 { get; set; }

        public string? ZZAEMNR { get; set; }

        public string? ZZREFNR { get; set; }

        public string? UMSSACHNR { get; set; }

        public string? SERIENNR { get; set; }

        public string? RMZHL { get; set; }

        public string? LGPLA { get; set; }

        public string? LOGO01 { get; set; }

        public string? LOGO02 { get; set; }

        public string? LOGO03 { get; set; }

        public string? LOGO04 { get; set; }

        public string? LOGO05 { get; set; }

        public string? LOGO06 { get; set; }

        public string? LOGO07 { get; set; }

        public string? LOGO08 { get; set; }

        public string? LOGO09 { get; set; }

        public string? LOGO10 { get; set; }

        public string? BSTNK { get; set; }

        public string? POSEX { get; set; }

        public string? FKALWO { get; set; }

        public string? FJAHR { get; set; }

        public string? KDMAT { get; set; }

        public string? ZUMCTEXT01 { get; set; }

        public string? ZUMCTEXT02 { get; set; }

        public string? ZUMCTEXT03 { get; set; }

        public string? KSMAT { get; set; }

        public string? AIRCRAFTTYPE { get; set; }

        public string? MSN { get; set; }

        public string? PARTNUMBER { get; set; }

        public string? KDMATBEZ { get; set; }

        public string? ZZBMNR { get; set; }

        public string? JIG { get; set; }

        public string? BI { get; set; }

        public string? CI { get; set; }

        public string? A_HTZ_NR { get; set; }

        public string? TWP { get; set; }

        public string? VBELN { get; set; }

        public string? EDATU { get; set; }

        public string? LIEFNAME { get; set; }

        public string? CHARG { get; set; }

        public string? A_AIRBUSBUNDLE { get; set; }

        public string? A_VERSION { get; set; }

        public string? A_AIRBUSBUNDLENAME { get; set; }

        public string? A_FLUGZEUGTYP { get; set; }

        public string? A_IDNR { get; set; }

        public string? A_SERIENNRB { get; set; }

        public string? A_SERIENNRV { get; set; }

        public string? CREVST { get; set; }

        public string? CAEMNR { get; set; }

        public string? CINTINDEX { get; set; }

        public string? ZUSATZFELD { get; set; }

        public string? NAME1 { get; set; }

        public string? NAME2 { get; set; }

        public string? NAME3 { get; set; }

        public string? NAME4 { get; set; }

        public string? STRAS { get; set; }

        public string? PSTLZ { get; set; }

        public string? ORT01 { get; set; }

        public string? LAND_D { get; set; }

        public string? LAND_E { get; set; }

        public string? CHARGE1 { get; set; }

        public string? DATCODE1 { get; set; }

        public string? QTY1 { get; set; }

        public string? CHARGE2 { get; set; }

        public string? DATCODE2 { get; set; }

        public string? QTY2 { get; set; }

        public string? QTYGES { get; set; }

        public string? BSTKD_E { get; set; }

        public string? MAKTXZ1 { get; set; }

        public string? GROES { get; set; }

        public string? GROESEINH { get; set; }

        public string? ZGROES { get; set; }

        public string? ZBARNUM { get; set; }

        public string? CSCOMMENT { get; set; }

        public string? CHARGE3 { get; set; }

        public string? DATCODE3 { get; set; }

        public string? QTY3 { get; set; }

        public string? CHARGE4 { get; set; }

        public string? DATCODE4 { get; set; }

        public string? QTY4 { get; set; }

        public string? CHARGE5 { get; set; }

        public string? DATCODE5 { get; set; }

        public string? QTY5 { get; set; }

        public string? LFIMG { get; set; }

        public string? BOXBUMBER { get; set; }

        public string? DELL_QTY { get; set; }

        public string? BSTKD { get; set; }
    }
}
