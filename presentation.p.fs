
\ Haeufig benutzte Textauszeichnungen:
: <f> <b> Blue <fc> ;
: </f> </fc> </b> ;

: p4-1
	<h> !" Welche Funktionen sind moeglich?" </h>
	<p>
		!" Natuerlich Text: " <f> s\" !\" Irgend ein Text\"" !! </f>
		<br>
		!" (Manchmal ist '" <f> s\" s\" Etwa wenn man !\" erklaeren will\" !!" !! </f> !" ' noetig)"
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

: farbendemo'' <fc> !" ====" ;
: farbendemo'
	7 0 +do
		i postpone literal postpone <bc>
		7 0 +do
			i postpone literal postpone farbendemo''
		loop
		postpone <br>
	loop
; immediate
: farbendemo   farbendemo' </bc> </fc> ;

256 Constant max-line
Create line-buffer  max-line 2 + allot

0 Value fd-in
: open-input ( addr u -- )  r/o open-file throw to fd-in ;

: printsource ( from to addr u -- )
   open-input
   cr
   0
   begin
      1+
      line-buffer max-line fd-in read-line throw
   while
	swap 2over rot swap over >= if
		swap over <= if
      			dup 0 U.R s" -> " type swap line-buffer swap type cr
		else swap drop
		endif
        else swap drop swap drop
	endif
   repeat 
   2drop 2drop 
   fd-in close-file throw ;

: printCodeHeader ( end start namelen addr -- )  \ prints source code header containing line numbers
  swap 2dup type 2swap \ s" (" type 0 U.R  s" -" type 0 U.R  s" ): " type cr cr type ;
  swap 2swap cr printsource cr ; \ type emit emit ;

: {source}  ( -- ) ;   
: <source>  ( -- , xt-{source}  )  ['] {source} , ;
: {/source} ( -- ) dup dup dup dup @ swap cell + @  2swap cell 2 * + 
		   @ swap cell 3 * + @ printCodeHeader 4 cells + ;
: </source> ( -- , xt-{/source} )  ['] {/source} , , , , , ;

<presentation>
	<h> !" Dies ist eine Testpraesentation!" </h>
	<p>
		!" Eines Tages hatten wir [" <b> <tw> !" Harald Steinlechner" </tw> </b>
		!"  und " <b> <tw> !" Denis Knauf" </tw> </b>
		!" ] die tolle Idee, eine Praesentationssoftware zu schreiben."
	</p>
	<p> <en>
                <||> !" Forth eignet sich gut für Domain specific languages."  </||> 
		<||> !" Makros sind ein Heidenspass." </||> 
		<||> !" Execution tokens erzeugen unseren Code."     </||> 
	</en> </p>
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
	<li> !" Hintergrundfarbe: " <f> s\" Yellow <bc> !\" text\" </bc> " !! </f> !" : " Yellow <bc> !\" text" </bc> </li>
	<li> !" Vordergrundfarbe: " <f> s\" Brown <fc> !\" text\" </fc> " !! </f> !" : " Brown <fc> !\" text" </fc> </li>
	<p> !" 8 Farben sind moeglich:" </p>
	<p> farbendemo </p>
<np>
	<h> !" Und Source" </h>
	<br>
	<source> s" presentation.fs" 10 20 </source>
</presentation>
