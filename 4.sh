#!/bin/bash

for filename in /home/xyz/*; do
	#gets first 4 Numbers from filename
	datestring=$(echo "${filename}" | grep -Po '\d{4}')
	year=${datestring:0:2}
	week=${datestring:2:2}
	directory="${year}/${week}/"
	mkdir -p "${directory}"
	mv -i -- "${filename}" "${directory}"
done


echo $ "Done!"