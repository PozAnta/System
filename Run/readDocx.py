from docx import Document

import sys

app = []
doc = Document('HomeRigidTest.docx')
for p in doc.paragraphs:
    print(p.text)
