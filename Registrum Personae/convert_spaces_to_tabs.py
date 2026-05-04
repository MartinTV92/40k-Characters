import os
import re

for root, dirs, files in os.walk('Assets'):
    for file in files:
        if file.endswith('.cs'):
            path = os.path.join(root, file)
            with open(path, 'r', encoding='utf-8') as f:
                content = f.read()
            # Replace leading spaces with tabs (4 spaces = 1 tab)
            new_content = re.sub(r'^ +', lambda m: '\t' * (len(m.group()) // 4), content, flags=re.MULTILINE)
            with open(path, 'w', encoding='utf-8') as f:
                f.write(new_content)

print("Conversion complete!")