#set term png
#set out "plot_dead-info-time_notl.png"
#set yrange [0:260]
#set xrange [0:4]
#set xtics( "30" 1, "40" 2, "60" 3)
#set key below
#set xlabel "car maximum speed [km/h]"
#set ylabel "time before deadlock [s]"
#set title "Time before dead-info-time without traffic lights"
#file = "plotdata_dead-info-time_notl.txt"
#plot file index 0 w linespoints title "only straight",\
#	 file index 1 w linespoints title "with turns"
#	 
#
#set term png
#set out "plot_dead-info-time_tl.png"
#set yrange [0:260]
#set xrange [0:4]
#set xtics( "30" 1, "40" 2, "60" 3)
#set key below
#set xlabel "car maximum speed [km/h]"
#set ylabel "time before deadlock [s]"
#set title "Time before dead-info-time with traffic lights (1 minute interval)"
#file = "plotdata_dead-info-time_tl.txt"
#plot file index 0 w linespoints lt rgb "red" title "straight",\
#	 file index 1 w linespoints lt rgb "orange" title "with turns"
#
#
#set term png
#set out "plot_dead-info-time_tl20.png"
#set yrange [0:260]
#set xrange [0:4]
#set xtics( "30" 1, "40" 2, "60" 3)
#set key below
#set xlabel "car maximum speed [km/h]"
#set ylabel "time before deadlock [s]"
#set title "Time before dead-info-time with traffic lights (20 seconds interval)"
#file = "plotdata_dead-info-time_tl20.txt"
#plot file index 0 w linespoints lt rgb "blue" title "straight",\
#	 file index 1 w linespoints lt rgb "gray" title "with turns"



set term png
set out "plot_dtimestraight.png"
set yrange [0:260]
set xrange [0:4]
set xtics( "30" 1, "40" 2, "60" 3)
set key below
set xlabel "car maximum speed [km/h]"
set ylabel "time before deadlock [s]"
set title "Time before deadlock cars going straight"
fileno = "plotdata_dead-info-time_notl.txt"
filetl = "plotdata_dead-info-time_tl.txt"
file20 = "plotdata_dead-info-time_tl20.txt"
plot fileno index 0 w linespoints lt rgb "green" title "pure reactive",\
	filetl index 0 w linespoints lt rgb "red" title "traffic lights 60 sec",\
	file20 index 0 w linespoints lt rgb "blue" title "traffic lights 20 sec"

set term png
set out "plot_dtimeturns.png"
set yrange [0:260]
set xrange [0:4]
set xtics( "30" 1, "40" 2, "60" 3)
set key below
set xlabel "car maximum speed [km/h]"
set ylabel "time before deadlock [s]"
set title "Time before deadlock cars making turns"
fileno = "plotdata_dead-info-time_notl.txt"
filetl = "plotdata_dead-info-time_tl.txt"
file20 = "plotdata_dead-info-time_tl20.txt"
plot fileno index 1 w linespoints lt rgb "green" title "pure reactive",\
	filetl index 1 w linespoints lt rgb "red" title "traffic lights 60 sec",\
	file20 index 1 w linespoints lt rgb "blue" title "traffic lights 20 sec"