
\ Haeufig benutzte Textauszeichnungen:
: <f> <b> 4 <fc> ;
: </f> </fc> </b> ;

: p4-1
	<h> !" Welche Funktionen sind moeglich?" </h>
	<p>
		!" Natuerlich Text: " <f> s\" !\" Irgend ein Text\"" !! </f>
		<br>
		!" (Manchmal ist '" <f> s\" s\" Etwa wenn man !\" erklaeren zu wollen\" !!" !! </f> !" ' noetig)"
	</p>
;
: p4-2
	<p> !" Aber " <u> !" immer" </u> !"  innerhalb eines Blockes:" </p>
	<li> <f> s\" <h> !\" Eine Ueberschrift\" </h>" !! </f> </li>
	<li> <f> s\" <p> !\" Einfacher Text\" </p>" !! </f> </li>
	<li> <f> s\" <li> !\" Listen, wie diese hier\" </li>" !! </f> </li>
	<p> !" Eine neue Seite definieren: " <f> !" <np>" </f> </li>
;

: p4-3
	<p> !" Textauszeichnung:" </p>
	<li> <f> s\" <b> !\" Fettdruck\" </b>" !! </f> !" : " <b> !" Fettdruck" </b> </li>
	<li> <f> s\" <i> !\" Farbinvertierung\" </i>" !! </f> !" : " <i> !" Farbinvertierung" </i> </li>
	<li> <f> s\" <u> !\" Unterstrichen\" </u>" !! </f> !" : " <u> !" Unterstrichen" </u> </li>
;

: farbendemo
	0 <bc> 0 <fc> !" ====" 1 <fc> !" ====" 2 <fc> !" ====" 3 <fc> !" ====" 4 <fc> !" ====" 5 <fc> !" ====" 6 <fc> !" ====" 7 <fc> !" ====" <br>
	1 <bc> 0 <fc> !" ====" 1 <fc> !" ====" 2 <fc> !" ====" 3 <fc> !" ====" 4 <fc> !" ====" 5 <fc> !" ====" 6 <fc> !" ====" 7 <fc> !" ====" <br>
	2 <bc> 0 <fc> !" ====" 1 <fc> !" ====" 2 <fc> !" ====" 3 <fc> !" ====" 4 <fc> !" ====" 5 <fc> !" ====" 6 <fc> !" ====" 7 <fc> !" ====" <br>
	3 <bc> 0 <fc> !" ====" 1 <fc> !" ====" 2 <fc> !" ====" 3 <fc> !" ====" 4 <fc> !" ====" 5 <fc> !" ====" 6 <fc> !" ====" 7 <fc> !" ====" <br>
	4 <bc> 0 <fc> !" ====" 1 <fc> !" ====" 2 <fc> !" ====" 3 <fc> !" ====" 4 <fc> !" ====" 5 <fc> !" ====" 6 <fc> !" ====" 7 <fc> !" ====" <br>
	5 <bc> 0 <fc> !" ====" 1 <fc> !" ====" 2 <fc> !" ====" 3 <fc> !" ====" 4 <fc> !" ====" 5 <fc> !" ====" 6 <fc> !" ====" 7 <fc> !" ====" <br>
	6 <bc> 0 <fc> !" ====" 1 <fc> !" ====" 2 <fc> !" ====" 3 <fc> !" ====" 4 <fc> !" ====" 5 <fc> !" ====" 6 <fc> !" ====" 7 <fc> !" ====" <br>
	7 <bc> 0 <fc> !" ====" 1 <fc> !" ====" 2 <fc> !" ====" 3 <fc> !" ====" 4 <fc> !" ====" 5 <fc> !" ====" 6 <fc> !" ====" 7 <fc> !" ===="
	</bc> </fc>
;

<presentation>
	<h> !" Dies ist eine Testpraesentation!" </h>
	<p>
		!" Eines Tages hatten wir [" <b> !" Harald Steinlechner" </b>
		!"  und " <b> !" Denis Knauf" </b>
		!" ] die tolle Idee, eine Praesentationssoftware zu schreiben."
	</p>
<np>
	<h> !" Ergebnis" </h>
	<p> <b> !" Das hier" </b> </p>
<np>
	<h> !" hallo" </h>
	<p> !" Sieht doch garnicht so schlecht aus" </p>
<np>
	p4-1
<np>
	p4-1 p4-2
<np>
	p4-1 p4-2 p4-3
<np>
	<h> !" Und Farben" </h>
	<br>
	<li> !" Hintergrundfarbe: " <f> !" 3 <bc>" </f> </li>
	<li> !" Vordergrundfarbe: " <f> !" 5 <fc>" </f> </li>
	<p> !" 8 Farben sind moeglich:" </p>
	<p> farbendemo </p>
</presentation>
