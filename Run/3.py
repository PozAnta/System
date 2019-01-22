

recset_file = ""
f = ""
new_value = "4"

try:
    recset_file = open('C:\\MC Projects\\UCI2\\StartPRG.PRG', "r")
    list_file = recset_file.read()
    recset_file.close()

    print(list_file.find("dim setup_con as long = 1"))

    if list_file.find("dim setup_con as long = 1") != -1:

        cut_end = list_file[list_file.find("Load AX_SETUP.PRG"):]
        cut_start = list_file[:list_file.find("dim setup_con as long = 1")]
        print(cut_start)
        print(cut_end)

        f = open('C:\\MC Projects\\UCI2\\Start.PRG', "w")
        f.write(cut_start)
        f.write("dim setup_con as long = " + new_value)
        f.write("\nselect case setup_con")

        f.write("\n case 1")
        f.write("\n  Load EC_SETUP.PRG")
        f.write("\n  Stas EC_SETUP.PRG")
        f.write("\n  while EC_SETUP.PRG.state <> 10")
        f.write("\n   sleep 10")
        f.write("\n  end while")

        f.write("\n case 2")
        f.write("\n  Load EC_SET_2.PRG")
        f.write("\n  Stas EC_SE_2.PRG")
        f.write("\n  while EC_SE_2.PRG.state <> 10")
        f.write("\n   sleep 10")
        f.write("\n  end while")

        f.write("\nend select\n")

        f.write("\n  " + cut_end)

finally:
    recset_file.close()
    f.close()
