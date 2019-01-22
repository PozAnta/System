import xml.etree.ElementTree as ET

tree = ET.parse('C:\\work\\Test\MyWPF\Run\\test1.xml')
root = tree.getroot()


def report():

    setup_id = "1"
    test_name = "Enable"

    count = 1
    items = ((0, '', '', ""),)

    if root.get('SetupId') == setup_id:  # Identification that test match to the setup
        print("SetupId= " + root.get('SetupId'))

        for root_group in root:

            print(" Attribute: TestId" + "= " + root_group.get("TestId"))

            if root_group.get("TestId") == test_name:  # Match the test into the setup
                print("Match Test!!")

                for test_root in root_group:
                    print(test_root.get('Name'))
                    print(test_root.get('Expect'))
                    print(test_root.text)


xml_obj = report()

