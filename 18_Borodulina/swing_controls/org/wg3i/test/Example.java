package org.wg3i.test;

import javax.swing.*;


public class Example extends JFrame {
    private JButton button;
    private JCheckBox checkBox;
    private JLabel label;
    private JTextField textField;

    public Example() {
        initUI();
    }

    public final void initUI() {
        button = new JButton("Button1");
        checkBox = new JCheckBox();
        label = new JLabel();
        textField = new JTextField(20);

        button.setBounds(50, 60, 100, 30);
        checkBox.setBounds(180, 60, 100, 30);
        label.setBounds(20, 20, 160, 40);
        textField.setBounds(50, 180, 140, 30);

        getContentPane().setLayout(null);
        getContentPane().add(button);
        getContentPane().add(checkBox);
        getContentPane().add(label);
        getContentPane().add(textField);

        button.addActionListener(e -> label.setText("button clicked"));

        checkBox.addActionListener(e -> label.setText("checkbox clicked"));

        setSize(300, 300);
        setLocationRelativeTo(null);
        setDefaultCloseOperation(EXIT_ON_CLOSE);
    }

    public void clickOnButton() {
        button.doClick();
    }

    public void clickOnCheckBox() {
        checkBox.doClick();
    }

    public void inputText(String text) {
        textField.setText(text);
    }

    public static void main(String[] args) {
        Example ex = new Example();
        ex.setVisible(true);
    }
}