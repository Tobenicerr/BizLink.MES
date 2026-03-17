using BizLink.MES.Domain.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    public class SapLabelDataComponent
    {
        /// <summary>
        /// Component: TDOBNAME - Name
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("TDOBNAME")]
        public string? TDOBNAME { get; set; }

        /// <summary>
        /// Component: MATNR - Material Number
        /// Data Type: CHAR, Length: 18
        /// 输入参数 (Input parameter: X)
        /// </summary>
        [MaxLength(18)]
        [SapFieldName("MATNR")]
        public string? MATNR { get; set; }

        /// <summary>
        /// Component: WERKS - Plant
        /// Data Type: CHAR, Length: 4
        /// 输入参数 (Input parameter: X)
        /// </summary>
        [MaxLength(4)]
        [SapFieldName("WERKS")]
        public string? WERKS { get; set; }

        /// <summary>
        /// Component: ETFORM
        /// Data Type: CHAR, Length: 8
        /// </summary>
        [MaxLength(8)]
        [SapFieldName("ETFORM")]
        public string? ETFORM { get; set; }

        /// <summary>
        /// Component: ETNR
        /// Data Type: CHAR, Length: 4
        /// </summary>
        [MaxLength(4)]
        [SapFieldName("ETNR")]
        public string? ETNR { get; set; }

        /// <summary>
        /// Component: SPRSL - Language Key
        /// Data Type: LANG, Length: 1
        /// </summary>
        [MaxLength(1)]
        [SapFieldName("SPRSL")]
        public string? SPRSL { get; set; }

        /// <summary>
        /// Component: ETFORMZ
        /// Data Type: CHAR, Length: 8
        /// </summary>
        [MaxLength(8)]
        [SapFieldName("ETFORMZ")]
        public string? ETFORMZ { get; set; }

        /// <summary>
        /// Component: ANZETP - Anzahl Etiketten (标签数量)
        /// Data Type: CHAR, Length: 2
        /// </summary>
        [MaxLength(2)]
        [SapFieldName("ANZETP")]
        public string? ANZETP { get; set; }

        /// <summary>
        /// Component: ANZET - Anzahl Etiketten (标签数量)
        /// Data Type: CHAR, Length: 2
        /// </summary>
        [MaxLength(2)]
        [SapFieldName("ANZET")]
        public string? ANZET { get; set; }

        /// <summary>
        /// Component: MULTITEXT - Single-Character Indicator
        /// Data Type: CHAR, Length: 1
        /// </summary>
        [MaxLength(1)]
        [SapFieldName("MULTITEXT")]
        public string? MULTITEXT { get; set; }

        /// <summary>
        /// Component: ZZFUNK
        /// Data Type: CHAR, Length: 4
        /// </summary>
        [MaxLength(4)]
        [SapFieldName("ZZFUNK")]
        public string? ZZFUNK { get; set; }

        /// <summary>
        /// Component: INTERNE
        /// Data Type: CHAR, Length: 4
        /// </summary>
        [MaxLength(4)]
        [SapFieldName("INTERNE")]
        public string? INTERNE { get; set; }

        /// <summary>
        /// Component: ARBGNR - Arbeitsgang FLR
        /// Data Type: CHAR, Length: 7
        /// </summary>
        [MaxLength(7)]
        [SapFieldName("ARBGNR")]
        public string? ARBGNR { get; set; }

        /// <summary>
        /// Component: AINDEX - Auftragsindex FLR
        /// Data Type: CHAR, Length: 3
        /// </summary>
        [MaxLength(3)]
        [SapFieldName("AINDEX")]
        public string? AINDEX { get; set; }

        /// <summary>
        /// Component: SAPAUF - Order Number
        /// Data Type: CHAR, Length: 12
        /// Mapping Content: Order Number
        /// </summary>
        [MaxLength(12)]
        [SapFieldName("SAPAUF")]
        public string? SAPAUF { get; set; }

        /// <summary>
        /// Component: AVO - SAP-Arbeitsvorgang
        /// Data Type: CHAR, Length: 4
        /// </summary>
        [MaxLength(4)]
        [SapFieldName("AVO")]
        public string? AVO { get; set; }

        /// <summary>
        /// Component: DATUM1
        /// Data Type: CHAR, Length: 5
        /// Mapping Content: Datum
        /// </summary>
        [MaxLength(5)]
        [SapFieldName("DATUM1")]
        public string? DATUM1 { get; set; }

        /// <summary>
        /// Component: DATUM2 - Datum
        /// Data Type: CHAR, Length: 8
        /// Mapping Content: Datum
        /// </summary>
        [MaxLength(8)]
        [SapFieldName("DATUM2")]
        public string? DATUM2 { get; set; }

        /// <summary>
        /// Component: LIEFL - Quantity
        /// Data Type: CHAR, Length: 5
        /// Mapping Content: Quantity
        /// </summary>
        [MaxLength(5)]
        [SapFieldName("LIEFL")]
        public string? LIEFL { get; set; }

        /// <summary>
        /// Component: SACHNR - Supplier reference number
        /// Data Type: CHAR, Length: 25
        /// </summary>
        [MaxLength(25)]
        [SapFieldName("SACHNR")]
        public string? SACHNR { get; set; }

        // --- 您提供的额外属性 (Additional properties you provided) ---

        /// <summary>
        /// Component: SACHNRK - Supplier reference number
        /// Data Type: CHAR, Length: 25
        /// </summary>
        [MaxLength(25)]
        [SapFieldName("SACHNRK")]
        public string? SACHNRK { get; set; }

        /// <summary>
        /// Component: MEINS - Base Unit of Measure
        /// Data Type: UNIT, Length: 3
        /// </summary>
        [MaxLength(3)]
        [SapFieldName("MEINS")]
        public string? MEINS { get; set; }

        /// <summary>
        /// Component: TYPBEZ01
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("TYPBEZ01")]
        public string? TYPBEZ01 { get; set; }

        /// <summary>
        /// Component: TYPBEZ02
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("TYPBEZ02")]
        public string? TYPBEZ02 { get; set; }

        /// <summary>
        /// Component: TYPBEZ03
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("TYPBEZ03")]
        public string? TYPBEZ03 { get; set; }

        /// <summary>
        /// Component: TYPBEZ04
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("TYPBEZ04")]
        public string? TYPBEZ04 { get; set; }

        /// <summary>
        /// Component: TYPBEZ05
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("TYPBEZ05")]
        public string? TYPBEZ05 { get; set; }

        /// <summary>
        /// Component: TYPBEZ06
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("TYPBEZ06")]
        public string? TYPBEZ06 { get; set; }

        /// <summary>
        /// Component: TYPBEZ07
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("TYPBEZ07")]
        public string? TYPBEZ07 { get; set; }

        /// <summary>
        /// Component: TYPBEZ08
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("TYPBEZ08")]
        public string? TYPBEZ08 { get; set; }

        /// <summary>
        /// Component: TYPBEZ09
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("TYPBEZ09")]
        public string? TYPBEZ09 { get; set; }

        /// <summary>
        /// Component: TYPBEZ10
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("TYPBEZ10")]
        public string? TYPBEZ10 { get; set; }

        /// <summary>
        /// Component: TYPBEK01
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("TYPBEK01")]
        public string? TYPBEK01 { get; set; }

        /// <summary>
        /// Component: TYPBEK02
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("TYPBEK02")]
        public string? TYPBEK02 { get; set; }

        /// <summary>
        /// Component: TYPBEK03
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("TYPBEK03")]
        public string? TYPBEK03 { get; set; }

        /// <summary>
        /// Component: TYPBEK04
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("TYPBEK04")]
        public string? TYPBEK04 { get; set; }

        /// <summary>
        /// Component: TYPBEK05
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("TYPBEK05")]
        public string? TYPBEK05 { get; set; }

        /// <summary>
        /// Component: TYPBEK06
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("TYPBEK06")]
        public string? TYPBEK06 { get; set; }

        /// <summary>
        /// Component: TYPBEK07
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("TYPBEK07")]
        public string? TYPBEK07 { get; set; }

        /// <summary>
        /// Component: TYPBEK08
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("TYPBEK08")]
        public string? TYPBEK08 { get; set; }

        /// <summary>
        /// Component: TYPBEK09
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("TYPBEK09")]
        public string? TYPBEK09 { get; set; }

        /// <summary>
        /// Component: TYPBEK10
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("TYPBEK10")]
        public string? TYPBEK10 { get; set; }

        /// <summary>
        /// Component: FARBE
        /// Data Type: CHAR, Length: 18
        /// </summary>
        [MaxLength(18)]
        [SapFieldName("FARBE")]
        public string? FARBE { get; set; }

        /// <summary>
        /// Component: FUNK
        /// Data Type: CHAR, Length: 1
        /// </summary>
        [MaxLength(1)]
        [SapFieldName("FUNK")]
        public string? FUNK { get; set; }

        /// <summary>
        /// Component: VDENR
        /// Data Type: CHAR, Length: 8
        /// </summary>
        [MaxLength(8)]
        [SapFieldName("VDENR")]
        public string? VDENR { get; set; }

        /// <summary>
        /// Component: TEXT01
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("TEXT01")]
        public string? TEXT01 { get; set; }

        /// <summary>
        /// Component: TEXT02
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("TEXT02")]
        public string? TEXT02 { get; set; }

        /// <summary>
        /// Component: TEXT03
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("TEXT03")]
        public string? TEXT03 { get; set; }

        /// <summary>
        /// Component: TEXT04
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("TEXT04")]
        public string? TEXT04 { get; set; }

        /// <summary>
        /// Component: TEXT05
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("TEXT05")]
        public string? TEXT05 { get; set; }

        /// <summary>
        /// Component: TEXT06
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("TEXT06")]
        public string? TEXT06 { get; set; }

        /// <summary>
        /// Component: TEXT07
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("TEXT07")]
        public string? TEXT07 { get; set; }

        /// <summary>
        /// Component: TEXT08
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("TEXT08")]
        public string? TEXT08 { get; set; }

        /// <summary>
        /// Component: TEXT09
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("TEXT09")]
        public string? TEXT09 { get; set; }

        /// <summary>
        /// Component: TEXT10
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("TEXT10")]
        public string? TEXT10 { get; set; }

        /// <summary>
        /// Component: TEXT11
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("TEXT11")]
        public string? TEXT11 { get; set; }

        /// <summary>
        /// Component: TEXT12
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("TEXT12")]
        public string? TEXT12 { get; set; }

        /// <summary>
        /// Component: TEXT13
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("TEXT13")]
        public string? TEXT13 { get; set; }

        /// <summary>
        /// Component: TEXT14
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("TEXT14")]
        public string? TEXT14 { get; set; }

        /// <summary>
        /// Component: TEXT15
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("TEXT15")]
        public string? TEXT15 { get; set; }

        /// <summary>
        /// Component: TEXT16
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("TEXT16")]
        public string? TEXT16 { get; set; }

        /// <summary>
        /// Component: TEXT17
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("TEXT17")]
        public string? TEXT17 { get; set; }

        /// <summary>
        /// Component: TEXT18
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("TEXT18")]
        public string? TEXT18 { get; set; }

        /// <summary>
        /// Component: TEXT19
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("TEXT19")]
        public string? TEXT19 { get; set; }

        /// <summary>
        /// Component: TEXT20
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("TEXT20")]
        public string? TEXT20 { get; set; }

        /// <summary>
        /// Component: TEXT21
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("TEXT21")]
        public string? TEXT21 { get; set; }

        /// <summary>
        /// Component: TEXT22
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("TEXT22")]
        public string? TEXT22 { get; set; }

        /// <summary>
        /// Component: TEXT23
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("TEXT23")]
        public string? TEXT23 { get; set; }

        /// <summary>
        /// Component: TEXT24
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("TEXT24")]
        public string? TEXT24 { get; set; }

        /// <summary>
        /// Component: TEXT25
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("TEXT25")]
        public string? TEXT25 { get; set; }

        /// <summary>
        /// Component: LAENGE01 - Länge
        /// Data Type: CHAR, Length: 6
        /// </summary>
        [MaxLength(6)]
        [SapFieldName("LAENGE01")]
        public string? LAENGE01 { get; set; }

        /// <summary>
        /// Component: FAKTOR01 - Längenfaktor
        /// Data Type: CHAR, Length: 5
        /// </summary>
        [MaxLength(5)]
        [SapFieldName("FAKTOR01")]
        public string? FAKTOR01 { get; set; }

        /// <summary>
        /// Component: TROMNR01
        /// Data Type: CHAR, Length: 12
        /// </summary>
        [MaxLength(12)]
        [SapFieldName("TROMNR01")]
        public string? TROMNR01 { get; set; }

        /// <summary>
        /// Component: TROMTP01
        /// Data Type: CHAR, Length: 18
        /// </summary>
        [MaxLength(18)]
        [SapFieldName("TROMTP01")]
        public string? TROMTP01 { get; set; }

        /// <summary>
        /// Component: MARK_A01 - Markierung - Anfang
        /// Data Type: CHAR, Length: 6
        /// </summary>
        [MaxLength(6)]
        [SapFieldName("MARK_A01")]
        public string? MARK_A01 { get; set; }

        /// <summary>
        /// Component: MARK_E01 - Markierung - Ende
        /// Data Type: CHAR, Length: 6
        /// </summary>
        [MaxLength(6)]
        [SapFieldName("MARK_E01")]
        public string? MARK_E01 { get; set; }

        /// <summary>
        /// Component: QKENNZ01
        /// Data Type: CHAR, Length: 10
        /// </summary>
        [MaxLength(10)]
        [SapFieldName("QKENNZ01")]
        public string? QKENNZ01 { get; set; }

        /// <summary>
        /// Component: LAENNR01
        /// Data Type: CHAR, Length: 12
        /// </summary>
        [MaxLength(12)]
        [SapFieldName("LAENNR01")]
        public string? LAENNR01 { get; set; }

        /// <summary>
        /// Component: KABELN01
        /// Data Type: CHAR, Length: 12
        /// </summary>
        [MaxLength(12)]
        [SapFieldName("KABELN01")]
        public string? KABELN01 { get; set; }

        /// <summary>
        /// Component: GEWEINH - Weight Unit
        /// Data Type: UNIT, Length: 3
        /// </summary>
        [MaxLength(3)]
        [SapFieldName("GEWEINH")]
        public string? GEWEINH { get; set; }

        /// <summary>
        /// Component: BRUTTO01
        /// Data Type: CHAR, Length: 10
        /// </summary>
        [MaxLength(10)]
        [SapFieldName("BRUTTO01")]
        public string? BRUTTO01 { get; set; }

        /// <summary>
        /// Component: TARAGE01
        /// Data Type: CHAR, Length: 10
        /// </summary>
        [MaxLength(10)]
        [SapFieldName("TARAGE01")]
        public string? TARAGE01 { get; set; }

        /// <summary>
        /// Component: KALWO - Version Number Component
        /// Data Type: CHAR, Length: 2
        /// </summary>
        [MaxLength(2)]
        [SapFieldName("KALWO")]
        public string? KALWO { get; set; }

        /// <summary>
        /// Component: JAHR - Version Number Component
        /// Data Type: CHAR, Length: 2
        /// </summary>
        [MaxLength(2)]
        [SapFieldName("JAHR")]
        public string? JAHR { get; set; }

        /// <summary>
        /// Component: ZZURSP01
        /// Data Type: CHAR, Length: 6
        /// </summary>
        [MaxLength(6)]
        [SapFieldName("ZZURSP01")]
        public string? ZZURSP01 { get; set; }

        /// <summary>
        /// Component: LAYOUT - Label Layout
        /// Data Type: CHAR, Length: 40
        /// </summary>
        [MaxLength(40)]
        [SapFieldName("LAYOUT")]
        public string? LAYOUT { get; set; }

        /// <summary>
        /// Component: DATUM3 - Datum tt.mm.jjjj Datum tt.mm.jjjj
        /// Data Type: CHAR, Length: 10
        /// </summary>
        [MaxLength(10)]
        [SapFieldName("DATUM3")]
        public string? DATUM3 { get; set; }

        /// <summary>
        /// Component: UMS01 - Umsetzfeld01
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("UMS01")]
        public string? UMS01 { get; set; }

        /// <summary>
        /// Component: UMS02 - Umsetzfeld02
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("UMS02")]
        public string? UMS02 { get; set; }

        /// <summary>
        /// Component: UMS03 - Umsetzfeld03
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("UMS03")]
        public string? UMS03 { get; set; }

        /// <summary>
        /// Component: UMS04 - Umsetzfeld04
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("UMS04")]
        public string? UMS04 { get; set; }

        /// <summary>
        /// Component: UMS05 - Umsetzfeld05
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("UMS05")]
        public string? UMS05 { get; set; }

        /// <summary>
        /// Component: UMS06 - Umsetzfeld06
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("UMS06")]
        public string? UMS06 { get; set; }

        /// <summary>
        /// Component: UMS07 - Umsetzfeld07
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("UMS07")]
        public string? UMS07 { get; set; }

        /// <summary>
        /// Component: UMS08 - Umsetzfeld08
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("UMS08")]
        public string? UMS08 { get; set; }

        /// <summary>
        /// Component: ZZAEMNR
        /// Data Type: CHAR, Length: 30
        /// </summary>
        [MaxLength(30)]
        [SapFieldName("ZZAEMNR")]
        public string? ZZAEMNR { get; set; }

        /// <summary>
        /// Component: ZZREFNR
        /// Data Type: CHAR, Length: 30
        /// </summary>
        [MaxLength(30)]
        [SapFieldName("ZZREFNR")]
        public string? ZZREFNR { get; set; }

        /// <summary>
        /// Component: UMSSACHNR - Supplier reference number
        /// Data Type: CHAR, Length: 25
        /// </summary>
        [MaxLength(25)]
        [SapFieldName("UMSSACHNR")]
        public string? UMSSACHNR { get; set; }

        /// <summary>
        /// Component: SERIENNR
        /// Data Type: CHAR, Length: 30
        /// </summary>
        [MaxLength(30)]
        [SapFieldName("SERIENNR")]
        public string? SERIENNR { get; set; }

        /// <summary>
        /// Component: RMZHL
        /// Data Type: CHAR, Length: 8
        /// </summary>
        [MaxLength(8)]
        [SapFieldName("RMZHL")]
        public string? RMZHL { get; set; }

        /// <summary>
        /// Component: LGPLA - Storage Bin
        /// Data Type: CHAR, Length: 10
        /// </summary>
        [MaxLength(10)]
        [SapFieldName("LGPLA")]
        public string? LGPLA { get; set; }

        /// <summary>
        /// Component: LOGO01
        /// Data Type: CHAR, Length: 20
        /// </summary>
        [MaxLength(20)]
        [SapFieldName("LOGO01")]
        public string? LOGO01 { get; set; }

        /// <summary>
        /// Component: LOGO02
        /// Data Type: CHAR, Length: 20
        /// </summary>
        [MaxLength(20)]
        [SapFieldName("LOGO02")]
        public string? LOGO02 { get; set; }

        /// <summary>
        /// Component: LOGO03
        /// Data Type: CHAR, Length: 20
        /// </summary>
        [MaxLength(20)]
        [SapFieldName("LOGO03")]
        public string? LOGO03 { get; set; }

        /// <summary>
        /// Component: LOGO04
        /// Data Type: CHAR, Length: 20
        /// </summary>
        [MaxLength(20)]
        [SapFieldName("LOGO04")]
        public string? LOGO04 { get; set; }

        /// <summary>
        /// Component: LOGO05
        /// Data Type: CHAR, Length: 20
        /// </summary>
        [MaxLength(20)]
        [SapFieldName("LOGO05")]
        public string? LOGO05 { get; set; }

        /// <summary>
        /// Component: LOGO06
        /// Data Type: CHAR, Length: 20
        /// </summary>
        [MaxLength(20)]
        [SapFieldName("LOGO06")]
        public string? LOGO06 { get; set; }

        /// <summary>
        /// Component: LOGO07
        /// Data Type: CHAR, Length: 20
        /// </summary>
        [MaxLength(20)]
        [SapFieldName("LOGO07")]
        public string? LOGO07 { get; set; }

        /// <summary>
        /// Component: LOGO08
        /// Data Type: CHAR, Length: 20
        /// </summary>
        [MaxLength(20)]
        [SapFieldName("LOGO08")]
        public string? LOGO08 { get; set; }

        /// <summary>
        /// Component: LOGO09
        /// Data Type: CHAR, Length: 20
        /// </summary>
        [MaxLength(20)]
        [SapFieldName("LOGO09")]
        public string? LOGO09 { get; set; }

        /// <summary>
        /// Component: LOGO10
        /// Data Type: CHAR, Length: 20
        /// </summary>
        [MaxLength(20)]
        [SapFieldName("LOGO10")]
        public string? LOGO10 { get; set; }

        /// <summary>
        /// Component: BSTNK - Bestellnummer des Kunden, Eingabe Kundenauftrag erforderlich
        /// Data Type: CHAR, Length: 35
        /// </summary>
        [MaxLength(35)]
        [SapFieldName("BSTNK")]
        public string? BSTNK { get; set; }

        /// <summary>
        /// Component: POSEX - Positionsnummer der Bestellung , Eingabe Kundenauftrag erf.
        /// Data Type: CHAR, Length: 6
        /// </summary>
        [MaxLength(6)]
        [SapFieldName("POSEX")]
        public string? POSEX { get; set; }

        /// <summary>
        /// Component: FKALWO - Version Number Component
        /// Data Type: CHAR, Length: 2
        /// </summary>
        [MaxLength(2)]
        [SapFieldName("FKALWO")]
        public string? FKALWO { get; set; }

        /// <summary>
        /// Component: FJAHR - Version Number Component
        /// Data Type: CHAR, Length: 2
        /// </summary>
        [MaxLength(2)]
        [SapFieldName("FJAHR")]
        public string? FJAHR { get; set; }

        /// <summary>
        /// Component: KDMAT - Material Number Used by Customer
        /// Data Type: CHAR, Length: 35
        /// </summary>
        [MaxLength(35)]
        [SapFieldName("KDMAT")]
        public string? KDMAT { get; set; }

        /// <summary>
        /// Component: ZUMCTEXT01 - Zusatz-MC-Text (Materialstamm Sprache Z)
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("ZUMCTEXT01")]
        public string? ZUMCTEXT01 { get; set; }

        /// <summary>
        /// Component: ZUMCTEXT02 - Zusatz-MC-Text (Materialstamm Sprache Z)
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("ZUMCTEXT02")]
        public string? ZUMCTEXT02 { get; set; }

        /// <summary>
        /// Component: ZUMCTEXT03 - Zusatz-MC-Text (Materialstamm Sprache Z)
        /// Data Type: CHAR, Length: 70
        /// </summary>
        [MaxLength(70)]
        [SapFieldName("ZUMCTEXT03")]
        public string? ZUMCTEXT03 { get; set; }

        /// <summary>
        /// Component: KSMAT - Material Number Used by Customer
        /// Data Type: CHAR, Length: 35
        /// </summary>
        [MaxLength(35)]
        [SapFieldName("KSMAT")]
        public string? KSMAT { get; set; }

        /// <summary>
        /// Component: AIRCRAFTTYPE - Aircrafttype
        /// Data Type: CHAR, Length: 40
        /// </summary>
        [MaxLength(40)]
        [SapFieldName("AIRCRAFTTYPE")]
        public string? AIRCRAFTTYPE { get; set; }

        /// <summary>
        /// Component: MSN
        /// Data Type: CHAR, Length: 12
        /// </summary>
        [MaxLength(12)]
        [SapFieldName("MSN")]
        public string? MSN { get; set; }

        /// <summary>
        /// Component: PARTNUMBER - Customer description of material
        /// Data Type: CHAR, Length: 40
        /// </summary>
        [MaxLength(40)]
        [SapFieldName("PARTNUMBER")]
        public string? PARTNUMBER { get; set; }

        /// <summary>
        /// Component: KDMATBEZ - Customer description of material
        /// Data Type: CHAR, Length: 40
        /// </summary>
        [MaxLength(40)]
        [SapFieldName("KDMATBEZ")]
        public string? KDMATBEZ { get; set; }

        /// <summary>
        /// Component: ZZBMNR - BM-Nummer
        /// Data Type: CHAR, Length: 16
        /// </summary>
        [MaxLength(16)]
        [SapFieldName("ZZBMNR")]
        public string? ZZBMNR { get; set; }

        /// <summary>
        /// Component: JIG - JIG-Nummer
        /// Data Type: CHAR, Length: 28
        /// </summary>
        [MaxLength(28)]
        [SapFieldName("JIG")]
        public string? JIG { get; set; }

        /// <summary>
        /// Component: BI - Bauteileindex
        /// Data Type: CHAR, Length: 30
        /// </summary>
        [MaxLength(30)]
        [SapFieldName("BI")]
        public string? BI { get; set; }

        /// <summary>
        /// Component: CI - Change Issure
        /// Data Type: CHAR, Length: 30
        /// </summary>
        [MaxLength(30)]
        [SapFieldName("CI")]
        public string? CI { get; set; }

        /// <summary>
        /// Component: A_HTZ_NR - HTZ Nummer
        /// Data Type: CHAR, Length: 30
        /// </summary>
        [MaxLength(30)]
        [SapFieldName("A_HTZ_NR")]
        public string? A_HTZ_NR { get; set; }

        /// <summary>
        /// Component: TWP - Technical Workpackage
        /// Data Type: CHAR, Length: 40
        /// </summary>
        [MaxLength(40)]
        [SapFieldName("TWP")]
        public string? TWP { get; set; }

        /// <summary>
        /// Component: VBELN - Kundenauftragsnummer
        /// Data Type: CHAR, Length: 10
        /// </summary>
        [MaxLength(10)]
        [SapFieldName("VBELN")]
        public string? VBELN { get; set; }

        /// <summary>
        /// Component: EDATU - Liefertermin
        /// Data Type: CHAR, Length: 10
        /// </summary>
        [MaxLength(10)]
        [SapFieldName("EDATU")]
        public string? EDATU { get; set; }

        /// <summary>
        /// Component: LIEFNAME - Name des Lieferanten
        /// Data Type: CHAR, Length: 30
        /// </summary>
        [MaxLength(30)]
        [SapFieldName("LIEFNAME")]
        public string? LIEFNAME { get; set; }

        /// <summary>
        /// Component: CHARG - Batch Number Batch Number
        /// Data Type: CHAR, Length: 10
        /// </summary>
        [MaxLength(10)]
        [SapFieldName("CHARG")]
        public string? CHARG { get; set; }

        /// <summary>
        /// Component: A_AIRBUSBUNDLE - A_AIRBUSBUNDLE
        /// Data Type: CHAR, Length: 30
        /// </summary>
        [MaxLength(30)]
        [SapFieldName("A_AIRBUSBUNDLE")]
        public string? A_AIRBUSBUNDLE { get; set; }

        /// <summary>
        /// Component: A_VERSION - A_VERSION
        /// Data Type: CHAR, Length: 30
        /// </summary>
        [MaxLength(30)]
        [SapFieldName("A_VERSION")]
        public string? A_VERSION { get; set; }

        /// <summary>
        /// Component: A_AIRBUSBUNDLENAME - A_AIRBUSBUNDLENAME
        /// Data Type: CHAR, Length: 30
        /// </summary>
        [MaxLength(30)]
        [SapFieldName("A_AIRBUSBUNDLENAME")]
        public string? A_AIRBUSBUNDLENAME { get; set; }

        /// <summary>
        /// Component: A_FLUGZEUGTYP - A_FLUGZEUGTYP
        /// Data Type: CHAR, Length: 30
        /// </summary>
        [MaxLength(30)]
        [SapFieldName("A_FLUGZEUGTYP")]
        public string? A_FLUGZEUGTYP { get; set; }

        /// <summary>
        /// Component: A_IDNR - A_IDNR
        /// Data Type: CHAR, Length: 30
        /// </summary>
        [MaxLength(30)]
        [SapFieldName("A_IDNR")]
        public string? A_IDNR { get; set; }

        /// <summary>
        /// Component: A_SERIENNRB - A_SERIENNRB
        /// Data Type: CHAR, Length: 30
        /// </summary>
        [MaxLength(30)]
        [SapFieldName("A_SERIENNRB")]
        public string? A_SERIENNRB { get; set; }

        /// <summary>
        /// Component: A_SERIENNRV - A_SERIENNRV
        /// Data Type: CHAR, Length: 30
        /// </summary>
        [MaxLength(30)]
        [SapFieldName("A_SERIENNRV")]
        public string? A_SERIENNRV { get; set; }

        /// <summary>
        /// Component: CREVST - Revision level
        /// Data Type: CHAR, Length: 6
        /// </summary>
        [MaxLength(6)]
        [SapFieldName("CREVST")]
        public string? CREVST { get; set; }

        /// <summary>
        /// Component: CAEMNR - Change notification number
        /// Data Type: CHAR, Length: 20
        /// </summary>
        [MaxLength(20)]
        [SapFieldName("CAEMNR")]
        public string? CAEMNR { get; set; }

        /// <summary>
        /// Component: CINTINDEX - Internal index
        /// Data Type: CHAR, Length: 4
        /// </summary>
        [MaxLength(4)]
        [SapFieldName("CINTINDEX")]
        public string? CINTINDEX { get; set; }

        /// <summary>
        /// Component: ZUSATZFELD - Zusatzfeld: Eingabe in zmetikm1
        /// Data Type: CHAR, Length: 30
        /// </summary>
        [MaxLength(30)]
        [SapFieldName("ZUSATZFELD")]
        public string? ZUSATZFELD { get; set; }

        /// <summary>
        /// Component: NAME1 - Name 1
        /// Data Type: CHAR, Length: 35
        /// </summary>
        [MaxLength(35)]
        [SapFieldName("NAME1")]
        public string? NAME1 { get; set; }

        /// <summary>
        /// Component: NAME2 - Name 2
        /// Data Type: CHAR, Length: 35
        /// </summary>
        [MaxLength(35)]
        [SapFieldName("NAME2")]
        public string? NAME2 { get; set; }

        /// <summary>
        /// Component: NAME3 - Name 3
        /// Data Type: CHAR, Length: 35
        /// </summary>
        [MaxLength(35)]
        [SapFieldName("NAME3")]
        public string? NAME3 { get; set; }

        /// <summary>
        /// Component: NAME4 - Name 4
        /// Data Type: CHAR, Length: 35
        /// </summary>
        [MaxLength(35)]
        [SapFieldName("NAME4")]
        public string? NAME4 { get; set; }

        /// <summary>
        /// Component: STRAS - House number and street
        /// Data Type: CHAR, Length: 35
        /// </summary>
        [MaxLength(35)]
        [SapFieldName("STRAS")]
        public string? STRAS { get; set; }

        /// <summary>
        /// Component: PSTLZ - Postal Code
        /// Data Type: CHAR, Length: 10
        /// </summary>
        [MaxLength(10)]
        [SapFieldName("PSTLZ")]
        public string? PSTLZ { get; set; }

        /// <summary>
        /// Component: ORT01 - City
        /// Data Type: CHAR, Length: 35
        /// </summary>
        [MaxLength(35)]
        [SapFieldName("ORT01")]
        public string? ORT01 { get; set; }

        /// <summary>
        /// Component: LAND_D - Bezeichnung des Landes (deutsch)
        /// Data Type: CHAR, Length: 50
        /// </summary>
        [MaxLength(50)]
        [SapFieldName("LAND_D")]
        public string? LAND_D { get; set; }

        /// <summary>
        /// Component: LAND_E - Bezeichnung des Landes (englisch)
        /// Data Type: CHAR, Length: 50
        /// </summary>
        [MaxLength(50)]
        [SapFieldName("LAND_E")]
        public string? LAND_E { get; set; }

        /// <summary>
        /// Component: CHARGE1 - Charge/FAUF-Nr. CISCO
        /// Data Type: CHAR, Length: 12
        /// </summary>
        [MaxLength(12)]
        [SapFieldName("CHARGE1")]
        public string? CHARGE1 { get; set; }

        /// <summary>
        /// Component: DATCODE1 - Verpackungsdatum CISCO
        /// Data Type: CHAR, Length: 4
        /// </summary>
        [MaxLength(4)]
        [SapFieldName("DATCODE1")]
        public string? DATCODE1 { get; set; }

        /// <summary>
        /// Component: QTY1 - Verpackungsmenge CISCO
        /// Data Type: CHAR, Length: 6
        /// </summary>
        [MaxLength(6)]
        [SapFieldName("QTY1")]
        public string? QTY1 { get; set; }

        /// <summary>
        /// Component: CHARGE2 - Charge/FAUF-Nr. CISCO
        /// Data Type: CHAR, Length: 12
        /// </summary>
        [MaxLength(12)]
        [SapFieldName("CHARGE2")]
        public string? CHARGE2 { get; set; }

        /// <summary>
        /// Component: DATCODE2 - Verpackungsdatum CISCO
        /// Data Type: CHAR, Length: 4
        /// </summary>
        [MaxLength(4)]
        [SapFieldName("DATCODE2")]
        public string? DATCODE2 { get; set; }

        /// <summary>
        /// Component: QTY2 - Verpackungsmenge CISCO
        /// Data Type: CHAR, Length: 6
        /// </summary>
        [MaxLength(6)]
        [SapFieldName("QTY2")]
        public string? QTY2 { get; set; }

        /// <summary>
        /// Component: QTYGES - Gesamtverpackungsmenge CISCO
        /// Data Type: CHAR, Length: 6
        /// </summary>
        [MaxLength(6)]
        [SapFieldName("QTYGES")]
        public string? QTYGES { get; set; }

        /// <summary>
        /// Component: BSTKD_E - Ship-to Party's Purchase Order Number
        /// Data Type: CHAR, Length: 35
        /// </summary>
        [MaxLength(35)]
        [SapFieldName("BSTKD_E")]
        public string? BSTKD_E { get; set; }

        /// <summary>
        /// Component: MAKTXZ1 - Material Description (Short Text)
        /// Data Type: CHAR, Length: 40
        /// </summary>
        [MaxLength(40)]
        [SapFieldName("MAKTXZ1")]
        public string? MAKTXZ1 { get; set; }

        /// <summary>
        /// Component: GROES - MC-Length
        /// Data Type: CHAR, Length: 13
        /// </summary>
        [MaxLength(13)]
        [SapFieldName("GROES")]
        public string? GROES { get; set; }

        /// <summary>
        /// Component: GROESEINH - MC-Length Unit
        /// Data Type: UNIT, Length: 3
        /// </summary>
        [MaxLength(3)]
        [SapFieldName("GROESEINH")]
        public string? GROESEINH { get; set; }

        /// <summary>
        /// Component: ZGROES - Size/dimensions
        /// Data Type: CHAR, Length: 32
        /// </summary>
        [MaxLength(32)]
        [SapFieldName("ZGROES")]
        public string? ZGROES { get; set; }

        /// <summary>
        /// Component: ZBARNUM - 17-Char. Field
        /// Data Type: CHAR, Length: 17
        /// </summary>
        [MaxLength(17)]
        [SapFieldName("ZBARNUM")]
        public string? ZBARNUM { get; set; }

        /// <summary>
        /// Component: CSCOMMENT - Text editor text line
        /// Data Type: CHAR, Length: 72
        /// </summary>
        [MaxLength(72)]
        [SapFieldName("CSCOMMENT")]
        public string? CSCOMMENT { get; set; }

        /// <summary>
        /// Component: CHARGE3 - Charge/FAUF-Nr. CISCO
        /// Data Type: CHAR, Length: 12
        /// </summary>
        [MaxLength(12)]
        [SapFieldName("CHARGE3")]
        public string? CHARGE3 { get; set; }

        /// <summary>
        /// Component: DATCODE3 - Verpackungsdatum CISCO
        /// Data Type: CHAR, Length: 4
        /// </summary>
        [MaxLength(4)]
        [SapFieldName("DATCODE3")]
        public string? DATCODE3 { get; set; }

        /// <summary>
        /// Component: QTY3 - Verpackungsmenge CISCO
        /// Data Type: CHAR, Length: 6
        /// </summary>
        [MaxLength(6)]
        [SapFieldName("QTY3")]
        public string? QTY3 { get; set; }

        /// <summary>
        /// Component: CHARGE4 - Charge/FAUF-Nr. CISCO
        /// Data Type: CHAR, Length: 12
        /// </summary>
        [MaxLength(12)]
        [SapFieldName("CHARGE4")]
        public string? CHARGE4 { get; set; }

        /// <summary>
        /// Component: DATCODE4 - Verpackungsdatum CISCO
        /// Data Type: CHAR, Length: 4
        /// </summary>
        [MaxLength(4)]
        [SapFieldName("DATCODE4")]
        public string? DATCODE4 { get; set; }

        /// <summary>
        /// Component: QTY4 - Verpackungsmenge CISCO
        /// Data Type: CHAR, Length: 6
        /// </summary>
        [MaxLength(6)]
        [SapFieldName("QTY4")]
        public string? QTY4 { get; set; }

        /// <summary>
        /// Component: CHARGE5 - Charge/FAUF-Nr. CISCO
        /// Data Type: CHAR, Length: 12
        /// </summary>
        [MaxLength(12)]
        [SapFieldName("CHARGE5")]
        public string? CHARGE5 { get; set; }

        /// <summary>
        /// Component: DATCODE5 - Verpackungsdatum CISCO
        /// Data Type: CHAR, Length: 4
        /// </summary>
        [MaxLength(4)]
        [SapFieldName("DATCODE5")]
        public string? DATCODE5 { get; set; }

        /// <summary>
        /// Component: QTY5 - Verpackungsmenge CISCO
        /// Data Type: CHAR, Length: 6
        /// </summary>
        [MaxLength(6)]
        [SapFieldName("QTY5")]
        public string? QTY5 { get; set; }

        /// <summary>
        /// Component: LFIMG - Quantity
        /// Data Type: CHAR, Length: 17
        /// </summary>
        [MaxLength(17)]
        [SapFieldName("LFIMG")]
        public string? LFIMG { get; set; }

        /// <summary>
        /// Component: BOXBUMBER - box number for dell box label
        /// Data Type: CHAR, Length: 30
        /// </summary>
        [MaxLength(30)]
        [SapFieldName("BOXBUMBER")]
        public string? BOXBUMBER { get; set; }

        /// <summary>
        /// Component: DELL_QTY - quantity of dell box
        /// Data Type: NUMC, Length: 3
        /// </summary>
        [MaxLength(3)]
        [SapFieldName("DELL_QTY")]
        public string? DELL_QTY { get; set; }

        /// <summary>
        /// Component: BSTKD - Customer purchase order number
        /// Data Type: CHAR, Length: 35
        /// </summary>
        [MaxLength(35)]
        [SapFieldName("BSTKD")]
        public string? BSTKD { get; set; }
    }
}
