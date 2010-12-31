#! /usr/bin/gforth
\ here-allokation wird als fifo verwendet.
: copy ( addrdst addrsrc len -- addrdstend )
	over ( dst src len src ) + swap ( dst end src )
	do ( dst+ )
		i ( dst+ src+ ) c@ ( dst+ chr )
		over ( dst+ chr dst+ ) c! ( dst+ ) 1+
	loop
;

: isnewline? ( c -- i ) dup 10 = swap 13 = or ;
: isspace?   ( c -- i ) dup 9 = over 11 = or swap 32 = or ;
: anyspaces? ( c -- i ) dup isnewline? isspace? or ;

: clearwspace ( c -- c )
	dup
	case
	9  of 32 endof
	11 of 32 endof
	13 of 10 endof
		dup
	endcase nip
;

variable ptype-lenl_
: ptype-lenl
	ptype-lenl_
	s\" \nptype-lenl = " type dup @ . cr
;
: ptype-word ( addrw addrc c -- addrc+1 )
	-rot \ c addrw addrc
	dup -rot over - type \ c addrc
	swap emit 1+ \ addrc+1
;
: ptype-init ( addr len lenm lenl -- addre lenm lenl addrw addre addr )
	{ addr len lenm lenl }  addr len +  lenm  lenl  addr  addr len +  addr
(	2over + \ addr len lenm lenl addre
	rot swap tuck 2-rot \ addre lenm addr len lenl addre
	2over drop rot swap \ addre lenm addr len addre lenl addr
	dup -rot 2-rot \ addre lenm lenl addrw addr len addre addr
	2nip \ addre lenm lenl addrw addre addr
)
;
: ptype-newline ( lenm lenl addrw addrc -- lenm 0 addrw )
	\ s\" is a newline\n" type
	10 ptype-word \ lenm lenl addrw=addrc+1
	nip 0 swap \ lenm lenl=0 addrw
;
: ptype-space ( lenm lenl addrw addrc -- lenm lenl1 addrw )
	\ s\" is a space\n" type
	32 ptype-word \ lenm lenl addrw=addrc+1
	swap 1+ swap \ lenm lenl+=1 addrw
;
: ptype-anychar ( lenm lenl addrw addrc -- lenm lenl addrw addrc )
	\ s\" => any char\n" type
	2over <=
	\ .s cr
	if
		\ lenm lenl addrw addrc
		rot tuck over swap - \ lenm addrw lenl addrc addrc-lenl \ m w l c c-l
		2over drop >= \ lenm addrw lenl addrc addrc-lenl>=addrw
		if \ Wort ist laenger als eine Zeile -> muss umgebrochen werden.
			1- -rot 1- -rot 2dup - \ lenm lenl addrc-1 addrw addrc-1-addrw
			type ." -" nip 1 swap dup \ lenm lenl addrw=addrc-1 addrc-1
		else \ Word erst in der naechsten Zeile ausgeben.
			nip 2dup - negate -rot \ lenm addrc-addrw addrw addrc
		then
		cr
	then
	rot 1+ -rot
	\ .s cr
	\ s\" <= any char\n" type
;
: ptype' ( addre lenm 0 addrw addre addr -- )
	\ .s cr
	\ addre ist fuer die schleife unwichtig
	+do \ lenm lenl addrw
		i dup c@ \ lenm lenl addrw addrc c
		clearwspace
		\ s\" loop>\n" type .s cr
		case \ lenm lenl addrw addrc c
		10 of ptype-newline endof
		32 of ptype-space endof
		drop ptype-anychar
		endcase
		\ .s cr
	loop \ addre lenm lenl addrw
	\ .s cr
	over ptype-lenl !
	nip nip tuck - type
;
: ptype ( addr len ) 80 ptype-lenl @ ptype-init ptype' ;
: ptype-reset ( -- ) 0 ptype-lenl ! ;
ptype-reset

: escape ( -- addr len ) s\" \e" ;
: csi ( -- addr len ) s\" \e[" ;
: sgr ( u -- ) csi type 0 0 d.r 109 ( m ) emit ;
: beep 7 emit s" *beep* " type ;

\ Es folgen ein paar blockorientierte Kennzeichnungen.
: {h}  ( addr -- addr ) cr s"    " type 3 ptype-lenl ! cell+ ; \ header
: <h>  ( -- addr u0 )  ['] {h}  , here 0 , 0 ;
: {/h} ( addr -- addr ) cr ;
: </h> ( addr len -- ) ['] {/h} , swap ! ;
: {p}  ( addr -- addr ) cr ptype-reset cell+ ; \ paragraph
: <p>  ( -- addr u0 )  ['] {p}  , here 0 , 0 ;
: {/p} ( addr -- addr ) cr ;
: </p> ( addr len -- ) ['] {/p} , swap ! ;
\ Es folgen ein paar syntaktische Textauszeichnungen.
: {i}   ( addr -- addr )  7 sgr ;
: <i>   ( -- ) ['] {i}  , ;
: {/i}  ( addr -- addr ) 27 sgr ;
: </i>  ( -- ) ['] {/i} , ;
: {b}   ( addr -- addr )  1 sgr ; \ bold
: <b>   ( -- ) ['] {b}  , ;
: {/b}  ( addr -- addr ) 22 sgr ;
: </b>  ( -- ) ['] {/b} , ;
: {u}   ( addr -- addr )  4 sgr ; \ underline
: <u>   ( -- ) ['] {u}  , ;
: {/u}  ( addr -- addr ) 24 sgr ;
: </u>  ( -- ) ['] {/u} , ;
: {fc}  ( addr -- addr ) dup @ 30 + sgr cell+ ; \ frontcolor
: <fc>  ( -- ) ['] {fc} , , ;
: {/fc} ( addr -- addr ) 39 sgr ;
: </fc> ( -- ) ['] {/fc} , ;
: {bc}  ( addr -- addr ) dup @ 40 + sgr cell+ ; \ backgroundcolor
: <bc>  ( -- ) ['] {bc} , , ;
: {/bc} ( addr -- addr ) 49 sgr ;
: </bc> ( -- ) ['] {/bc} , ;

: {np} ( -- )
	0 sgr \ Alle Bildschirmeigenschaften zuruecksetzen
	\ csi type s\" 2J" type \ Bildschirm leeren
;
: <np> ( -- addr ) \ Wir legen jede Anfangsadresse einer Seite auf den Stack (Achtung, in umgekehrter Reihenfolge
	here ['] {np} ,
;

: {!!} ( addr -- addr+2 )
	dup @   \ addr straddr
	swap    \ straddr addr
	cell+   \ straddr addr
	tuck    \ addr straddr addr
	@       \ addr straddr strlen
	ptype-init ptype'   \ addr
	cell+
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
	begin swap dup \ 0 <addr..> addr0 faddr
	while , \ 0 <addr..> faddr
	repeat \ .s cr
	drop \ faddr
	here dup 0 \ faddr laddr paddr 0
;
: pres_page_cur ( addr -- addr ) ;
: pres_page_from ( addr -- addr ) cell+ ;
: pres_page_to ( addr -- addr ) 2 cells + ;

\ faddr: erste Seitenzeigeradresse (letzte Presentationsseite)
\ laddr: letzte Seitenzeigeradresse (erste Presentationsseite)
\ paddr: derzeitige Seitenzeigeradresse

: page_steps ( laddr paddr 0 [u] -- u )
	\ u muss ungleich 0 sein. falls u nicht vorhanden: 1
	dup 0= if 1 then \ laddr paddr 0 u
	nip
;
: validpage? ( faddr laddr paddr -- faddr laddr paddr u )
	2dup <= \ faddr laddr paddr u
	2over drop rot tuck >
	rot \ faddr laddr paddr u u
	if drop cell - -1 \ faddr laddr paddr-1 -1
	else if cell+ -1 \ faddr laddr paddr+1 -1
	else 0 \ faddr laddr paddr 0
	then then
;
: showpage' ( paddr -- )
	dup cell - \ paddr paddr+cell
	@ swap @ \ naddr addr \ Seiteninhaltsadressen
	begin 2dup >
	while
		dup cell+ swap \ naddr xtaddr xtaddr
		@ \ naddr xtaddr xt
		execute \ verschiebt eventuell den Zeiger noch weiter, wenn es Parameter erwartet.
	repeat
	2drop
;
: showpage ( faddr laddr paddr -- faddr laddr paddr 0 )
	cells -
	validpage?
	if beep then
	dup showpage' 0
;
: n ( faddr laddr paddr 0 [u] -- faddr laddr paddr 0 )
	page_steps \ faddr laddr paddr x
	showpage
;
: g ( faddr laddr paddr 0 u -- faddr laddr paddr 0 )
	showpage
;
: p ( faddr laddr paddr 0 [u] -- faddr laddr paddr 0 )
	page_steps negate
	showpage
;

here
<presentation>
<h> s" Dies ist eine Testpresentation!" !! </h>
<p>
	s" Eines Tages hatten wir [" !! <b> s" Harald Steinlechner" !! </b>
	s"  und " !! <b> s" Denis Knauf" !! </b>
	s" ] die tolle Idee, eine Presentationssoftware zu schreiben." !!
</p>
<np>
<h> s" Ergebnis" !! </h>
<p> <b> s" Das hier" !! </b> </p>
<np>
<h> s" hallo" !! </h>
<p> s" Sieht doch garnicht so schlecht aus" !! </p>
</presentation>

( bye
\ presentation ist gestartet: erste Seite wird angezeigt
n \ zweite Seite
p \ erste Seite
2 n \ dritte Seite
)
