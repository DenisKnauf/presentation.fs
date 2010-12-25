#! /usr/bin/gforth
\ here-allokation wird als fifo verwendet.
: copy ( addrdst addrsrc len -- addrdstend )
	over ( dst src len src ) + swap ( dst end src )
	do ( dst+ )
		i ( dst+ src+ ) c@ ( dst+ chr )
		over ( dst+ chr dst+ ) c! ( dst+ ) 1+
	loop
;

: csi ( -- ) 27 emit 91 emit ;
: sgr ( u -- ) csi 0 0 d.r 109 emit ;
: beep 7 emit s" *beep* " type ;
\ Es folgen ein paar blockorientierte Kennzeichnungen.
: {h}  ( addr -- addr ) cell+ ; \ header
: {/h} ( addr -- addr ) ;
: {p}  ( addr -- addr ) cell+ ; \ paragraph
: {/p} ( addr -- addr ) ;
: <h>  ( -- addr u0 )  ['] {h}  , here 0 ;
: </h> ( addr len -- ) ['] {/h} , swap ! ;
: <p>  ( -- addr u0 )  ['] {p}  , here 0 ;
: </p> ( addr len -- ) ['] {/p} , swap ! ;
\ Es folgen ein paar syntaktische Textauszeichnungen.
: {i}   ( addr -- addr )  7 sgr ;
: {/i}  ( addr -- addr ) 27 sgr ;
: {b}   ( addr -- addr )  1 sgr ; \ bold
: {/b}  ( addr -- addr ) 22 sgr ;
: {u}   ( addr -- addr )  4 sgr ; \ underline
: {/u}  ( addr -- addr ) 24 sgr ;
: {fc}  ( addr -- addr ) dup @ 30 + sgr cell+ ; \ frontcolor
: {/fc} ( addr -- addr ) 39 sgr ;
: {bc}  ( addr -- addr ) dup @ 40 + sgr cell+ ; \ backgroundcolor
: {/bc} ( addr -- addr ) 49 sgr ;
: <i>   ( -- ) ['] {i}  , ;
: </i>  ( -- ) ['] {/i} , ;
: <u>   ( -- ) ['] {u}  , ;
: </u>  ( -- ) ['] {/u} , ;
: <b>   ( -- ) ['] {b}  , ;
: </b>  ( -- ) ['] {/b} , ;
: <fc>  ( -- ) ['] {fc} , , ;
: </fc> ( -- ) ['] {/fc} , ;
: <bc>  ( -- ) ['] {bc} , , ;
: </bc> ( -- ) ['] {/bc} , ;

: {np} ( -- )
	0 sgr \ Alle Bildschirmeigenschaften zuruecksetzen
	\ csi s" 2J" type \ Bildschirm leeren
;
: <np> ( -- addr ) \ Wir legen jede Anfangsadresse einer Seite auf den Stack (Achtung, in umgekehrter Reihenfolge
	here ['] {np} ,
;
: {!!} ( addr -- addr+2 )
	s" string-> " type .s newline type
	dup @ \ addr straddr
	cell+ \ straddr addr
	tuck @ \ addr straddr strlen
	type \ addr
	cell+
	s" string<- " type .s newline type
;
: !! ( len0 addr1 len1 -- len !! '{!!} addr1 len1 )
	['] {!!} ,
	dup \ len0 addr1 len1 len1
	rot , , \ len0 len1 len1 addr1 -> len0 len1
	+ \ len0+len1
;

: <presentation> ( -- 0 addr0 !! '{np} ) 0 here ['] {np} , ;
: </presentation> ( 0 <addr...> -- faddr laddr paddr 0 !! endaddr 0 0 0 0 <...addr> )
	here \ 0 <addr...> faddr
	0 , 0 , 0 , 0 ,
	begin swap dup \ 0 <addr..> addr0 faddr
	while , \ 0 <addr..> faddr
	repeat .s
	drop \ faddr
	here dup 0 \ faddr laddr paddr 0
;
: pres_page_cur ( addr -- addr ) ;
: pres_page_from ( addr -- addr ) cell+ ;
: pres_page_to ( addr -- addr ) 2 cells + ;

\ faddr: erste Seitenzeigeradresse (letzte Presentationsseite)
\ laddr: letzte Seitenzeigeradresse (erste Presentationsseite)
\ paddr: derzeitige Seitenzeigeradresse

: page_steps ( laddr paddr 0 [u] -- laddr naddr )
	\ u muss ungleich 0 sein. falls u nicht vorhanden: 1
	dup 0= if 1 then \ laddr paddr 0 u
	nip cells - .s
;
: validpage? ( faddr laddr paddr -- faddr laddr paddr u )
	2dup < \ faddr laddr paddr u
	2over drop rot tuck >=
	rot \ faddr laddr paddr u u
	if drop cell - -1 \ faddr laddr paddr-1 -1
	else if cell+ -1 \ faddr laddr paddr+1 -1
	else 0 \ faddr laddr paddr 0
	then then
;
: showpage' ( paddr -- )
	dup cell - .s \ paddr paddr+cell
	@ swap @ \ naddr addr \ Seiteninhaltsadressen
	begin
		s" ++ " type 2dup . .
		2dup >
		dup . newline type
	while
		dup cell+ swap \ naddr xtaddr xtaddr
		@ \ naddr xtaddr xt
		s" -> " type .s newline type
		execute \ verschiebt eventuell den Zeiger noch weiter, wenn es Parameter erwartet.
		s" <- " type .s newline type
	repeat
	2drop
;
: showpage ( faddr laddr paddr -- faddr laddr paddr 0 )
	validpage?
	if beep then
	dup showpage' 0
;
: n ( faddr laddr paddr 0 [u] -- faddr laddr paddr 0 )
	page_steps \ faddr laddr paddr x
	showpage
;
: g ( faddr laddr paddr 0 u -- faddr laddr paddr 0 )
	cells
	showpage
;
: p ( faddr laddr paddr 0 [u] -- faddr laddr paddr 0 )
	page_steps negate
	showpage
;

here . newline type
<presentation>
<h> s" Dies ist eine Testpresentation!" !! </h>
<p>
	s" Eines Tages hatten wir (" !! <i> s" Harald Steinlechner" !! </i>
	s" und" !! <i> s" Denis Knauf" !! </i>
	s" die tolle Idee, eine Presentationssoftware zu schreiben" !!
</p>
<np>
<h> s" Ergebnis:" !! </h>
<p> <b> s" Das hier" !! </b> </p>
<np>
<p> s" Sieht doch garnicht so schlecht aus" !! </p>
</presentation>

( bye
\ presentation ist gestartet: erste Seite wird angezeigt
n \ zweite Seite
p \ erste Seite
2 n \ dritte Seite
)
