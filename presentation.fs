#! /usr/bin/gforth
\ 
\ here-allokation wird als fifo verwendet.
\ 
: copy ( addrdst addrsrc len -- addrdstend )
	over ( dst src len src ) + swap ( dst end src )
	do ( dst+ )
		i ( dst+ src+ ) c@ ( dst+ chr )
		over ( dst+ chr dst+ ) c! ( dst+ ) 1+
	loop
;

: <presentation> ( -- 0 addr0 ) 0 here ;
: </presentation> ( 0 <addr...> -- faddr laddr paddr 0 )
	here .s \ 0 <addr...> faddr
	begin swap dup \ 0 <addr..> addr0 faddr
	while
		s" New page at " type here . s" to " type dup . newline type
		, \ 0 <addr..> faddr
	repeat .s
	drop \ faddr
	here dup 0 \ faddr laddr paddr 0
;

: csi ( -- ) 27 emit 91 emit ;
: sgr ( u -- ) csi 0 0 d.r 109 emit ;
: beep 7 type ;
\ Es folgen ein paar blockorientierte Kennzeichnungen.
: {h}  ( addr -- addr ) ;
: {/h} ( addr -- addr ) ;
: {p}  ( addr -- addr ) ;
: {/p} ( addr -- addr ) ;
: <h>  ( -- addr u0 )  ['] {h} , here 0 ;
: </h> ( addr len -- ) ['] {/h} , swap ! ;
: <p>  ( -- addr u0 )  ['] {p} , here 0 ;
: </p> ( addr len -- ) ['] {/p} , swap ! ;
\ Es folgen ein paar syntaktische Textauszeichnungen.
: {i}   ( addr -- addr )  7 sgr ;
: {/i}  ( addr -- addr ) 27 sgr ;
: {b}   ( addr -- addr )  1 sgr ;
: {/b}  ( addr -- addr ) 22 sgr ;
: {u}   ( addr -- addr )  4 sgr ;
: {/u}  ( addr -- addr ) 24 sgr ;
: {fc}  ( addr -- addr ) dup @ 30 + sgr 1 cells + ;
: {/fc} ( addr -- addr ) 39 sgr ;
: {bc}  ( addr -- addr ) dup @ 40 + sgr 1 cells + ;
: {/bc} ( addr -- addr ) 49 sgr ;
: <i>   ( -- ) ['] {i}   , ;
: </i>  ( -- ) ['] {/i}  , ;
: <u>   ( -- ) ['] {u}   , ;
: </u>  ( -- ) ['] {/u}  , ;
: <b>   ( -- ) ['] {b}   , ;
: </b>  ( -- ) ['] {/b}  , ;
: <fc>  ( -- ) ['] {fc}  , ;
: </fc> ( -- ) ['] {/fc} , ;
: <bc>  ( -- ) ['] {bc}  , ;
: </bc> ( -- ) ['] {/bc} , ;

: <np> ( -- addr ) \ Wir legen jede Anfangsadresse einer Seite auf den Stack (Achtung, in umgekehrter Reihenfolge
	here
;
\ : <+> ( addr1 len1 addr2 len2 -- addrdst lendst )
\	rot 2dup + here ( addr1 addr2 len2 len1 lendst addrdst )
\	2-rot -rot ( lendst addrdst addr1 len1 addr2 len2 )
\	2swap 2rot ( addr2 len2 addr1 len1 lendst addrdst )
\	2dup chars allot ( dst allocated )
\	copy copy
\ ;
: !! ( len0 addr1 len1 -- len )
	1 ,
	dup \ len0 addr1 len1 len1
	rot , , \ len0 len1 len1 addr1 -> len0 len1
	+
;

\ faddr: erste Seitenzeigeradresse (letzte Presentationsseite)
\ laddr: letzte Seitenzeigeradresse (erste Presentationsseite)
\ paddr: derzeitige Seitenzeigeradresse

: page_steps ( laddr paddr 0 [u] -- laddr naddr )
	\ u muss ungleich 0 sein. falls u nicht vorhanden: 1
	dup 0= if 1 then \ laddr paddr 0 u
	nip cells -
;
: validpage? ( faddr laddr paddr -- faddr laddr paddr u )
	2dup > \ faddr laddr paddr u
	2over drop rot tuck <
	rot and
;
: showpage' ( paddr -- )
	dup cell+ \ paddr paddr+cell
	@ swap @ \ naddr addr \ Seiteninhaltsadressen
	0 sgr \ Alle Bildschirmeigenschaften zuruecksetzen
	\ csi s" 2J" type \ Bildschirm leeren
	begin 2dup >
	while
		dup cell+ swap \ naddr xtaddr xtaddr
		@ \ naddr xtaddr xt
		execute \ verschiebt eventuell den Zeiger noch weiter, wenn es Parameter erwartet.
	repeat
	drop drop
;
: showpage ( faddr laddr paddr -- faddr laddr paddr 0 )
	validpage?
	0= if beep then
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
	negate n
;

<presentation>
<h> s" Dies ist eine Testpresentation" !! </h>
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
