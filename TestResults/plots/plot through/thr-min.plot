set term png
set out "plot_throughmin_notl.png"
set yrange [0:13]
set xrange [0:4]
set xtics( "30" 1, "40" 2, "60" 3)
set key below
set xlabel "car maximum speed [km/h]"
set ylabel "throughput [cars/min]"
set title "Throughput without traffic lights"
file = "plotdata_throughmin_notl.txt"
plot file index 0 w linespoints title "only straight",\
	 file index 1 w linespoints title "with turns"
	 

set term png
set out "plot_throughmin_tl.png"
set yrange [0:13]
set xrange [0:4]
set xtics( "30" 1, "40" 2, "60" 3)
set key below
set xlabel "car maximum speed [km/h]"
set ylabel "throughput [cars/min]"
set title "Throughput with traffic lights (1 minute interval)"
file = "plotdata_throughmin_tl.txt"
plot file index 0 w linespoints lt rgb "red" title "straight",\
	 file index 1 w linespoints lt rgb "orange" title "with turns"


set term png
set out "plot_throughmin_tl20.png"
set yrange [0:13]
set xrange [0:4]
set xtics( "30" 1, "40" 2, "60" 3)
set key below
set xlabel "car maximum speed [km/h]"
set ylabel "throughput [cars/min]"
set title "Throughput with traffic lights (20 seconds interval)"
file = "plotdata_throughmin_tl20.txt"
plot file index 0 w linespoints lt rgb "blue" title "straight",\
	 file index 1 w linespoints lt rgb "gray" title "with turns"